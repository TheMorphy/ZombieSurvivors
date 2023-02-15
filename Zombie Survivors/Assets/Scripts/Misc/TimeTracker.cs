using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Trackable
{
	public string TrackingCode;
	public float TrackingValue = -1;
	public bool IsTrackingTime = false;
}

public class TimeTracker : MonoBehaviour
{
	public static TimeTracker Instance;

	public List<Trackable> trackables= new List<Trackable>();

	private void Awake()
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(this);
	}

	public void InitializeTrackable(Slot slot)
	{
		Trackable trackable = new Trackable()
		{
			TrackingCode = slot.TrackingKey,
			TrackingValue = slot.UnlockTimer
		};

		trackables.Add(trackable);
	}

	private IEnumerator TrackTime(Trackable trackable)
	{
		trackable.IsTrackingTime = true;
		WaitForSeconds wait = new WaitForSeconds(1f);

		while (true)
		{
			trackable.TrackingValue -= 1f;
			yield return wait;
		}
	}

	/// <summary>
	/// When player presses Open collected airdrop, it stores the slot tracking key,
	/// which is used to keep tarck of time during the gameplay and after the game is closed.
	/// </summary>
	public void StartTrackingTime(Slot slotToTrackTime)
	{
		PlayerPrefs.SetString(slotToTrackTime.TrackingKey, slotToTrackTime.TrackingKey);

		Trackable trackable = trackables.First(trackable => trackable.TrackingCode == slotToTrackTime.TrackingKey);

		StartCoroutine(TrackTime(trackable));
	}

	public float GetSlotTime(string slotKey)
	{
		var trackable = trackables.First(x => x.TrackingCode.Equals(slotKey));

		return trackable.TrackingValue;
	}

	public void ClearTime(string trackingCode)
	{
		Utilities.DeletePrefs(new string[] {
		$"{trackingCode}_QuitTimeYear",
		$"{trackingCode}_QuitTimeMonth",
		$"{trackingCode}_QuitTimeDay",
		$"{trackingCode}_QuitTimeHour",
		$"{trackingCode}_QuitTimeMinute",
		$"{trackingCode}_QuitTimeSecond",
		$"{trackingCode}_SecondsLeft",
		$"{trackingCode}",
		});
	}

	private void SaveTime(Trackable trackable)
	{
		trackable.IsTrackingTime = false;

		DateTime quitTime = DateTime.UtcNow.ToLocalTime();

		int year = quitTime.Year;
		int month = quitTime.Month;
		int days = quitTime.Day;
		int hours = quitTime.Hour;
		int minutes = quitTime.Minute;
		int seconds = quitTime.Second;

		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeYear", year);
		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeMonth", month);
		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeDay", days);
		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeHour", hours);
		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeMinute", minutes);
		PlayerPrefs.SetInt($"{trackable.TrackingCode}_QuitTimeSecond", seconds);
		PlayerPrefs.SetFloat($"{trackable.TrackingCode}_SecondsLeft", trackable.TrackingValue);
	}

	public float GetSavedTime(string trackingCode)
	{
		Trackable trackable = trackables.Find(x => x.TrackingCode == trackingCode);

		if (trackable.IsTrackingTime)
			return trackable.TrackingValue;

		int year = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeYear");
		int month = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeMonth");
		int day = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeDay");
		int hours = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeHour");
		int minutes = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeMinute");
		int seconds = PlayerPrefs.GetInt($"{trackingCode}_QuitTimeSecond");

		float secondsLeft = PlayerPrefs.GetFloat($"{trackingCode}_SecondsLeft");

		DateTime savedDate = new (year, month, day, hours, minutes, seconds);

		var diff = (DateTime.UtcNow.ToLocalTime() - savedDate).TotalSeconds;

		trackable.TrackingValue = secondsLeft;

		trackable.TrackingValue -= (float)diff;

		return trackable.TrackingValue;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			foreach (var trackable in trackables)
			{
				if(trackable.IsTrackingTime)
				{
					SaveTime(trackable);
				}
			}
		}
	}
}
