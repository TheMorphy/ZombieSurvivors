using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SlotView))]
public class Slot : MonoBehaviour
{
	[SerializeField] private SlotView slotView;

	[HideInInspector] public AirdropDTO AirdropDetails;
	public bool IsEmpty = true;
	public bool TimerStarted = false;
	public float UnlockTimer;
	public string TrackingKey = "";
	public int SlotID;

	public void InitializeAirdropSlot(AirdropDTO airdropDetails, int slotIndex)
	{
		IsEmpty = false;
		AirdropDetails = airdropDetails;

		SetSlotReference();
		TrackingKey = $"{airdropDetails.AirdropType.ToString() + "_" + slotIndex}";

		TimeTracker.Instance.InitializeTrackable(this);

		if (PlayerPrefs.HasKey(TrackingKey))
		{
			print("Timer was started");
			TimerStarted = true;
			UnlockTimer = TimeTracker.Instance.GetSavedTime(TrackingKey);

			if (UnlockTimer <= 0)
			{
				print("Chest is finished opening");
				slotView.InitialiseViewUIForUnlockedChest();
			}
			else
			{
				print("Chest is waiting for time to finish");
				slotView.StartUnlockingAirdrop();
			}
		}
		else
		{
			print("Chest is waiting to be opened");
			UnlockTimer = airdropDetails.UnlockDuration;
			slotView.InitialiseViewUIForLockedChest();
		}
	}

	public void SetEmptySlot()
	{
		IsEmpty = true;
		SetSlotReference();
		slotView.InitializeEmptyChestView();
	}

	public void SetSlotReference()
	{
		slotView.SlotReference = this;
	}

	public void RemoveTime(float time)
	{
		UnlockTimer -= time;

		if (UnlockTimer <= 0)
		{
			slotView.InitialiseViewUIForLockedChest();
		}
	}

	public SlotView GetSlotView() 
	{
		return slotView;
	}
}
