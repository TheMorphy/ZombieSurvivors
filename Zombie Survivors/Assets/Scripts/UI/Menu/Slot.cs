using System;
using System.Collections;
using UnityEngine;

public class Slot : MonoBehaviour
{
	[SerializeField] private SlotView airdropView;
	public bool IsEmpty;
	[HideInInspector]
	public AirdropDetails AirdropDetails;

	private float unlockTimer;
	[HideInInspector] public bool isTimerRunning = false;

	[HideInInspector] public string SlotKey = "";

	private void Start()
	{
		SetSlotReference();
		IsEmpty = true;
	}

	public void SetAirdropSlot(AirdropDetails airdropDetails, int slotIndex)
	{
		AirdropDetails = Instantiate(airdropDetails);

		if (PlayerPrefs.HasKey(transform.name)) 
		{
			int days = PlayerPrefs.GetInt($"{transform.name}_QuitTimeDay");
			int hours = PlayerPrefs.GetInt($"{transform.name}_QuitTimeHour");
			int minutes = PlayerPrefs.GetInt($"{transform.name}_QuitTimeMinute");
			int seconds = PlayerPrefs.GetInt($"{transform.name}_QuitTimeSecond");

			float secondsLeft = PlayerPrefs.GetFloat($"{transform.name}_SecondsLeft");

			DateTime date = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, days, hours, minutes, seconds);

			var diff = (DateTime.UtcNow - date).TotalSeconds;

			unlockTimer = secondsLeft;

			unlockTimer -= (float)diff;

			if (unlockTimer <= 0)
			{
				airdropView.EnteringUnlockedState();
			}
			else
			{
				airdropView.EnteringUnlockingState();
			}
		}
		else
		{
			unlockTimer = airdropDetails.UnlockDuration;
			airdropView.InitialiseViewUIForLockedChest();
		}
		SlotKey = $"{airdropDetails.AirdropType.ToString() + "_" + slotIndex}";
		IsEmpty = false;
	}

	public void SetSlotReference()
	{
		airdropView.SlotReference = this;
	}

	public IEnumerator StartTimer(float startTime = -1)
	{
		isTimerRunning = true;
		// Acts like a seconds timer. If value is 1f, then it waits for 1 second to decrease the unlockTimer
		WaitForSeconds waitTime = new WaitForSeconds(1f);

		while (unlockTimer > 0)
		{
			int days = (int)(unlockTimer / 86400) % 365;
			int hours = (int)(unlockTimer / 3600) % 24;
			int minutes = (int)(unlockTimer / 60) % 60;
			int seconds = (int)(unlockTimer % 60);

			airdropView.chestTimerTxt.text = "";
			if (days > 0) { airdropView.chestTimerTxt.text += days + "d "; }
			if(hours > 0) { airdropView.chestTimerTxt.text += hours + "h "; }
			if(minutes > 0) { airdropView.chestTimerTxt.text += minutes + "m "; }
			if(seconds > 0) { airdropView.chestTimerTxt.text += seconds + "s "; }

			yield return waitTime;

			if(startTime != -1)
				unlockTimer -= startTime;
			else
				unlockTimer -= 1;
		}
		isTimerRunning = false;
		airdropView.EnteringUnlockedState();
	}
	public void SaveOpeniningTime()
	{
		DateTime quitTime = DateTime.UtcNow;

		int days = quitTime.Day;
		int hours = quitTime.Hour;
		int minutes = quitTime.Minute;
		int seconds = quitTime.Second;

		PlayerPrefs.SetInt($"{transform.name}_QuitTimeDay", days);
		PlayerPrefs.SetInt($"{transform.name}_QuitTimeHour", hours);
		PlayerPrefs.SetInt($"{transform.name}_QuitTimeMinute", minutes);
		PlayerPrefs.SetInt($"{transform.name}_QuitTimeSecond", seconds);

		PlayerPrefs.SetFloat($"{transform.name}_SecondsLeft", unlockTimer);

		PlayerPrefs.SetString(transform.name, transform.name);
	}

	private void OnApplicationPause(bool pause)
	{
		if (isTimerRunning && pause)
		{
			SaveOpeniningTime();
		}
	}
}
