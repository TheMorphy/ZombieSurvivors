using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SlotView))]
[System.Serializable]
public class Slot : MonoBehaviour
{
	[SerializeField] private SlotView slotView;

	[HideInInspector] public AirdropDTO AirdropDetailsDTO;
	public bool IsEmpty = true;
	public bool TimerStarted = false;
	public float UnlockTimer;
	public string TrackingKey = "";
	public int SlotID;

	public void InitializeAirdropSlot(AirdropDTO airdropDetailsDTO, int slotIndex)
	{
		AirdropDetailsDTO = airdropDetailsDTO;
		TrackingKey = $"{airdropDetailsDTO.AirdropType.ToString() + "_" + slotIndex}";
		IsEmpty = false;
		SetSlotReference();

		if (TimeTracker.Instance.Trackables.Any(x => x.TrackingCode == TrackingKey))
		{
			TimerStarted = true;
			TimeTracker.Instance.ContinueTimer(this);
			slotView.InitialiseViewUIForUnlockingChest();
		}
		else
		{
			UnlockTimer = airdropDetailsDTO.RemoveTime;
			slotView.InitialiseViewUIForLockedChest();
		}

		TimeTracker.Instance.OnGamePaused += Instance_OnGamePaused;
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

	private void OnDestroy()
	{
		print("OnDestroy");
		//	TimeTracker.Instance.OnGamePaused -= Instance_OnGamePaused;
	}

	private void Instance_OnGamePaused(bool IsPaused)
	{
		if(IsPaused)
		{
			TimeTracker.Instance.SaveTime(TrackingKey);
		}
		else
		{
			TimeTracker.Instance.ContinueTimer(this);
		}
	}
}
