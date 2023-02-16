using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SlotView))]
[System.Serializable]
public class Slot : MonoBehaviour
{
	[SerializeField] private SlotView slotView;
	[HideInInspector] public Trackable TrackableReference;
	[HideInInspector] public AirdropDTO AirdropDetailsDTO;

	public bool IsEmpty = true;
	public int UnlockTimer;
	public bool TimerStarted = false;
	public string TrackingKey = "";
	public int SlotID;

	private void Awake()
	{
		slotView.SlotReference = this;
	}

	public void InitializeAirdropSlot(AirdropDTO airdropDetailsDTO, int slotIndex)
	{
		AirdropDetailsDTO = airdropDetailsDTO;
		TrackingKey = $"{airdropDetailsDTO.AirdropType.ToString() + "_" + slotIndex}";
		IsEmpty = false;

		TrackableReference = TimeTracker.Instance.GetTrackable(TrackingKey);

		if (TrackableReference != null)
		{
			print("Continue Timer: " + TrackableReference.TrackingCode);
			TimerStarted = true;
			slotView.InitialiseViewUIForUnlockingChest();
		}
		else
		{
			TimerStarted = false;
			UnlockTimer = airdropDetailsDTO.RemoveTime;
			slotView.InitialiseViewUIForLockedChest();
		}
	}

	public void StartTracking()
	{
		TimeTracker.Instance.SetNewStrackable(TrackingKey, UnlockTimer);
		slotView.InitialiseViewUIForUnlockingChest();
	}

	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().InitializeWindow(this);
	}

	public void OpenChest()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
		SetEmptySlot();
	}

	public void SetEmptySlot()
	{
		IsEmpty = true;
		TimerStarted = false;
		UnlockTimer = 0;
		slotView.InitializeEmptyChestView();
	}

	public SlotView GetSlotView() 
	{
		return slotView;
	}
}
