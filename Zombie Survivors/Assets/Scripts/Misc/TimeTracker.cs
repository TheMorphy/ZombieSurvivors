using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// This approach uses UTCNow time, to hopefully store the universal time,
// that can't be altered by modifying system time

// P.S using local databse like SQLite would also be an option. But im too lazy to rewrite it
#region Trackable Object
[Serializable]
public class Trackable
{
	public int ID;
	public bool IsTracking = false;
	public int RemainingSeconds;
	public string TimerText;
	public string SaveDate;
	public MonoBehaviour monoBehaviour;

	public override bool Equals(object obj)
	{
		if (obj == null || !this.GetType().Equals(obj.GetType()))
		{
			return false;
		}

		Trackable other = (Trackable)obj;
		return this.ID == other.ID;
	}

	public override int GetHashCode()
	{
		return this.ID.GetHashCode();
	}


	public Trackable(MonoBehaviour monoBehaviour)
	{
		this.monoBehaviour = monoBehaviour;
	}

	public IEnumerator StartTimer()
	{
		WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
		while (RemainingSeconds > 0)
		{
			int days = (RemainingSeconds / 86400) % 365;
			int hours = (RemainingSeconds / 3600) % 24;
			int minutes = (RemainingSeconds / 60) % 60;
			int seconds = (RemainingSeconds % 60);

			TimerText = "";

			if (days > 0) { TimerText += days + "d "; }
			if (hours > 0) { TimerText += hours + "h "; }
			if (minutes > 0) { TimerText += minutes + "m "; }
			if (seconds > 0) { TimerText += seconds + "s "; }

			RemainingSeconds -= 1;
			SaveDate = DateTime.UtcNow.ToString();
			SaveManager.SaveToJSON(this, Settings.TRACKABLES);
			yield return wait;
		}
	}
}
#endregion


public class TimeTracker : MonoBehaviour
{
	public static TimeTracker Instance;

	public List<Trackable> Trackables { get; private set; } = new List<Trackable>();

	public float GameTime { get; private set; } = 0;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(this);

		Trackables = SaveManager.ReadFromJSON<Trackable>(Settings.TRACKABLES);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveTime();
			StopAllCoroutines();
		}
		else
		{
			ContinueTimers();
		}
	}

	public void TrackGameTime(float time)
	{
		GameTime = time;
		StartCoroutine(Track());
	}

	private IEnumerator Track()
	{
		while(GameTime > 0)
		{
			GameTime -= Time.deltaTime;
			yield return null;
		}	
	}

	public string GetFormattedGameTime()
	{
		float minutes = Mathf.FloorToInt(GameTime / 60);
		float seconds = Mathf.FloorToInt(GameTime % 60);

		if(minutes < 0 ) minutes = 0;
		if (seconds < 0) seconds = 0;

		return string.Format("{0:00} : {1:00}", minutes, seconds);
	}

	public Trackable GetTrackable(int trackingCode)
	{
		return Trackables.FirstOrDefault(x => x.ID == trackingCode);
	}
	/// <summary>
	/// When player presses Open collected airdrop, it stores the slot tracking key,
	/// which is used to keep tarck of time during the gameplay and after the game is closed.
	/// </summary>
	public void InitializeTrackable(int trackingID)
	{
		Trackable trackable = new Trackable(this)
		{
			ID = trackingID,
			RemainingSeconds = 0,
			IsTracking = false
		};

		Trackables.Add(trackable);
		SaveManager.SaveToJSON(trackable, Settings.TRACKABLES);
	}

	/// <summary>
	/// Resets Trackabale object
	/// </summary>
	public void ClearTime(int ID)
	{
		Trackable trackableToReset = GetTrackable(ID);
		trackableToReset.IsTracking = false;
		trackableToReset.TimerText = "";
		trackableToReset.SaveDate = "";
		trackableToReset.RemainingSeconds = 0;
		StopCoroutine(trackableToReset.StartTimer());
		SaveManager.SaveToJSON(trackableToReset, Settings.TRACKABLES);
	}

	/// <summary>
	/// Saves current local time to json file on application pause or close.
	/// </summary>
	private void SaveTime()
	{
		foreach (var trackable in Trackables.Where(x => x.IsTracking))
		{
			SaveManager.SaveToJSON(trackable, Settings.TRACKABLES);
		}
	}

	/// <summary>
	/// This is most definetly not be a good approach, bcause if timezone changes it can produce incorrect results.
	/// </summary>
	public int GetSavedRemainingSeconds(int ID)
	{
		Trackable trackable = GetTrackable(ID);
		TimeSpan timeSpan = DateTime.UtcNow - DateTime.Parse(trackable.SaveDate);
		int difference = trackable.RemainingSeconds - (int)timeSpan.TotalSeconds;

		return difference;
	}

	private void ContinueTimers()
	{
		foreach (var trackable in Trackables.Where(x => x.IsTracking))
		{
			trackable.RemainingSeconds = GetSavedRemainingSeconds(trackable.ID);
			StartCoroutine(trackable.StartTimer());
		}
	}

	public void DecreaseTime(int ID, int decreaseAmmount)
	{
		Trackable trackable = GetTrackable(ID);

		if(trackable != null)
		{
			trackable.RemainingSeconds -= decreaseAmmount;
		}
	}

	public void StartTrackingTime(int slotID, int unlockDuration)
	{
		Trackable toTrack = GetTrackable(slotID);
		toTrack.IsTracking = true;
		toTrack.RemainingSeconds = unlockDuration;
		StartCoroutine(toTrack.StartTimer());
	}
}
