using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Trackable
{
	public string TrackingCode = string.Empty;
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

public class TimeTracker : MonoBehaviour
{
	public static TimeTracker Instance;
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

			Trackables.ForEach(trackable => {
				if (string.IsNullOrEmpty(trackable.TrackingCode) == false)
				{
					SaveTime(trackable.TrackingCode);
				}
			});
		}
		else
		{
			Trackables.ForEach(trackable => {
				if (string.IsNullOrEmpty(trackable.TrackingCode) == false)
				{
					ContinueTimer(trackable);
				}
			});
		}
	}

	public Trackable? GetTrackable(string trackingCode)
	{
		return Trackables.FirstOrDefault(x => x.TrackingCode == trackingCode);
	}

	/// <summary>
	/// When player presses Open collected airdrop, it stores the slot tracking key,
	/// which is used to keep tarck of time during the gameplay and after the game is closed.
	/// </summary>
	public void SetNewStrackable(string trackingCode, int unlockTimer)
	{
		Trackable trackable = new Trackable(this)
		{
			TrackingCode = trackingCode,
			RemainingSeconds = unlockTimer
		};

		Trackables.Add(trackable);
		StartCoroutine(trackable.StartTimer());
	}

	/// <summary>
	/// Stops tracking the time
	/// </summary>
	private void StopTrackingTime(string trackingCode)
	{
		Trackable trackableToDelete = GetTrackable(trackingCode);
		StopCoroutine(trackableToDelete.StartTimer());
	}

	public void ClearTime(string trackingCode)
	{
		Trackable trackableToDelete = GetTrackable(trackingCode);
		StopTrackingTime(trackingCode);

		Trackables.Remove(trackableToDelete);

		SaveManager.DeleteFromJSON(trackableToDelete, Settings.TRACKABLES);
	}

	/// <summary>
	/// Save time on application pause or close
	/// </summary>
	public void SaveTime(string trackingKey)
	{
		var trackabaleToSave = GetTrackable(trackingKey);

		if (trackabaleToSave != null)
		{
			trackabaleToSave.SaveDate = DateTime.Now;

			if(SaveManager.GetNumSavedItems<Trackable>(Settings.TRACKABLES) > 0)
			{
				SaveManager.UpdateTrackingInJSON(trackabaleToSave, trackingKey, Settings.TRACKABLES);
			}
			else
			{
				SaveManager.SaveToJSON(trackabaleToSave, Settings.TRACKABLES);
			}
		}
	}

	public float GetRemainingSeconds(string trackingCode)
	{
		Trackable trackable = GetTrackable(trackingCode);
		TimeSpan timeSpan = DateTime.Now - trackable.SaveDate;
		trackable.RemainingSeconds -= (float)timeSpan.TotalSeconds;

		return trackable.RemainingSeconds;
	}

	public void ContinueTimer(Trackable trackable)
	{
		trackable.RemainingSeconds = GetRemainingSeconds(trackable.TrackingCode);
		StartCoroutine(trackable.StartTimer());
	}

	public void DecreaseTime(string trackingCode, int decreaseAmmount)
	{
		Trackable trackable = GetTrackable(trackingCode);

		if(trackable != null)
		{
			trackable.RemainingSeconds -= decreaseAmmount;
		}
	}
}
