using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SlotView))]
[System.Serializable]
public class Slot : MonoBehaviour
{
	[SerializeField] private SlotView slotView;
	[HideInInspector] public Trackable TrackableReference;
	[HideInInspector] public AirdropDTO AirdropDetailsDTO;

	public bool IsEmpty = true;
	public bool TimerStarted = false;
	public string TrackingKey = "";

	private void Update()
	{
		if(TrackableReference != null && TimerStarted)
		{
			if (TrackableReference.RemainingSeconds <= 0)
			{
				UnlockChest();
			}
			else
			{
				slotView.unlockTimeText.text = TrackableReference.TimerText;
			}
		}
	}

	public void InitializeAirdropSlot(AirdropDTO airdropDetailsDTO, int slotIndex)
	{
		slotView.SlotReference = this;
		AirdropDetailsDTO = airdropDetailsDTO;
		TrackingKey = $"{airdropDetailsDTO.AirdropType.ToString() + "_" + slotIndex}";
		IsEmpty = false;

		TrackableReference = TimeTracker.Instance.GetTrackable(TrackingKey);

		if (TrackableReference != null)
		{
			TimerStarted = true;
			slotView.InitialiseViewUIForUnlockingChest();
		}
		else
		{
			TimerStarted = false;
			slotView.InitialiseViewUIForLockedChest();
		}
	}

	public void StartTracking()
	{
		TimerStarted = true;
		TrackableReference = TimeTracker.Instance.SetNewStrackable(TrackingKey, AirdropDetailsDTO.UnlockDuration);
		slotView.InitialiseViewUIForUnlockingChest();
	}

	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().InitializeWindow(this);
	}

	public void UnlockChest()
	{
		slotView.InitialiseViewUIForUnlockedChest();
		TimerStarted = false;
	}

	public void OpenChest()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
	}

	public void SetEmptySlot()
	{
		IsEmpty = true;
		TimerStarted = false;
		TrackingKey = "";
		TrackableReference = null;
		AirdropDetailsDTO = null;
		slotView.InitializeEmptyChestView();
	}
}
