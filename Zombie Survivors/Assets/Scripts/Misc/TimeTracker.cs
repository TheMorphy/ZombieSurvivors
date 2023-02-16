using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Trackable
{
	public string TrackingCode;
	public float RemainingSeconds;
	public DateTime SaveDate;
	public MonoBehaviour monoBehaviour;

	public Trackable(MonoBehaviour monoBehaviour)
	{
		this.monoBehaviour = monoBehaviour;
	}

	public IEnumerator StartTimer(Slot slot)
	{
		SlotView slotView = slot.GetSlotView();

		WaitForSeconds wait = new WaitForSeconds(1f);
		while (RemainingSeconds > 0)
		{
			int days = (int)(RemainingSeconds / 86400) % 365;
			int hours = (int)(RemainingSeconds / 3600) % 24;
			int minutes = (int)(RemainingSeconds / 60) % 60;
			int seconds = (int)(RemainingSeconds % 60);

			slotView.unlockTimeText.text = "";

			if (days > 0) { slotView.unlockTimeText.text += days + "d "; }
			if (hours > 0) { slotView.unlockTimeText.text += hours + "h "; }
			if (minutes > 0) { slotView.unlockTimeText.text += minutes + "m "; }
			if (seconds > 0) { slotView.unlockTimeText.text += seconds + "s "; }

			RemainingSeconds -= 1;
			yield return wait;
		}
	}
}

public class TimeTracker : MonoBehaviour
{
	public static TimeTracker Instance;
	public List<Trackable> Trackables = new List<Trackable>();
	public event Action<bool> OnGamePaused;

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
		}

		OnGamePaused?.Invoke(pause);
	}

	/// <summary>
	/// When player presses Open collected airdrop, it stores the slot tracking key,
	/// which is used to keep tarck of time during the gameplay and after the game is closed.
	/// </summary>
	public void SetNewStrackable(Slot slot)
	{
		PlayerPrefs.SetString(slot.TrackingKey, slot.TrackingKey);

		Trackable trackable = new Trackable(this)
		{
			TrackingCode = slot.TrackingKey,
			RemainingSeconds = slot.UnlockTimer
		};

		Trackables.Add(trackable);

		StartCoroutine(trackable.StartTimer(slot));
	}

	public float GetSlotTime(string slotKey)
	{
		var trackable = Trackables.First(x => x.TrackingCode.Equals(slotKey));

		return trackable.RemainingSeconds;
	}

	public void ClearTime(string trackingKey)
	{
		Trackable trackableToDelete = Trackables.Find(x => x.TrackingCode.Equals(trackingKey));

		SaveManager.DeleteFromJSON(trackableToDelete, Settings.TRACKABLES);
	}

	public void SaveTime(string trackingKey)
	{
		var trackabaleToSave = Trackables.FirstOrDefault(x => x.TrackingCode == trackingKey);

		if (trackabaleToSave != null)
		{
			trackabaleToSave.SaveDate = DateTime.Now;

			if(SaveManager.GetNumSavedItems<Trackable>(Settings.TRACKABLES) > 0)
			{
				SaveManager.UpdateInJSON(trackabaleToSave, trackingKey, Settings.TRACKABLES);
			}
			else
			{
				SaveManager.SaveToJSON(trackabaleToSave, Settings.TRACKABLES);
			}
			
		}
	}

	public float GetRemainingSeconds(string trackingCode)
	{
		Trackable trackable = Trackables.FirstOrDefault(x => x.TrackingCode == trackingCode);

		TimeSpan timeSpan = DateTime.Now - trackable.SaveDate;

		trackable.RemainingSeconds = (float)timeSpan.TotalSeconds;

		return trackable.RemainingSeconds;
	}

	public void ContinueTimer(Slot slot)
	{
		var trackable = Trackables.First(x => x.TrackingCode == slot.TrackingKey);

		trackable.RemainingSeconds = GetRemainingSeconds(slot.TrackingKey);
		StartCoroutine(trackable.StartTimer(slot));
	}
}
