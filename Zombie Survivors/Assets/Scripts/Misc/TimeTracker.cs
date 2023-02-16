using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#region Trackable Object
[System.Serializable]
public class Trackable
{
	public int ID;
	public float RemainingSeconds;
	public string TimerText;
	public DateTime SaveDate;
	public MonoBehaviour monoBehaviour;

	public Trackable(MonoBehaviour monoBehaviour)
	{
		this.monoBehaviour = monoBehaviour;
	}

	public IEnumerator StartTimer()
	{
		WaitForSeconds wait = new WaitForSeconds(1f);
		while (RemainingSeconds > 0)
		{
			int days = (int)(RemainingSeconds / 86400) % 365;
			int hours = (int)(RemainingSeconds / 3600) % 24;
			int minutes = (int)(RemainingSeconds / 60) % 60;
			int seconds = (int)(RemainingSeconds % 60);

			TimerText = "";

			if (days > 0) { TimerText += days + "d "; }
			if (hours > 0) { TimerText += hours + "h "; }
			if (minutes > 0) { TimerText += minutes + "m "; }
			if (seconds > 0) { TimerText += seconds + "s "; }

			RemainingSeconds -= 1;
			yield return wait;
		}
	}
}
#endregion

public class TimeTracker : MonoBehaviour
{
	public static TimeTracker Instance;

	[Header("READONLY")]
	public List<Trackable> Trackables = new List<Trackable>();

	private void Awake()
	{
		Trackables = SaveManager.ReadFromJSON<Trackable>(Settings.TRACKABLES);

		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(this);
	}
	
	private void OnApplicationPause(bool pause)
	{
		if(pause)
		{
			StopAllCoroutines();
			SaveTime();
		}
		else
		{
			ContinueTimers();
		}
	}

	public Trackable GetTrackable(int trackingCode)
	{
		return Trackables.FirstOrDefault(x => x.ID == trackingCode);
	}
	/// <summary>
	/// When player presses Open collected airdrop, it stores the slot tracking key,
	/// which is used to keep tarck of time during the gameplay and after the game is closed.
	/// </summary>
	public Trackable SetNewStrackable(int trackingID, int unlockTimer)
	{
		Trackable trackable = new Trackable(this)
		{
			ID = trackingID,
			RemainingSeconds = unlockTimer
		};

		Trackables.Add(trackable);
		StartCoroutine(trackable.StartTimer());

		return trackable;
	}

	public void ClearTime(int ID)
	{
		Trackable trackableToDelete = GetTrackable(ID);
		StopCoroutine(trackableToDelete.StartTimer());
		SaveManager.DeleteFromJSON<Trackable>(ID, Settings.TRACKABLES);
		Trackables.Remove(trackableToDelete);
	}


	/// <summary>
	/// Save time on application pause or close
	/// </summary>
	public void SaveTime()
	{
		Trackables.ForEach(x => x.SaveDate = DateTime.Now);
		SaveManager.SaveToJSON<Trackable>(Trackables, Settings.TRACKABLES);
	}

	public float GetSavedRemainingSeconds(int ID)
	{
		Trackable trackable = GetTrackable(ID);
		TimeSpan timeSpan = DateTime.Now - trackable.SaveDate;
		trackable.RemainingSeconds -= (float)timeSpan.TotalSeconds;

		return trackable.RemainingSeconds;
	}

	public void ContinueTimers()
	{
		foreach (var trackable in Trackables)
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
}
