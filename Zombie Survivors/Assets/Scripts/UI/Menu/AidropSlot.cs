using UnityEngine;

[RequireComponent(typeof(AidropSlotView))]
public class AidropSlot : Slot<AirdropDTO>
{
	[SerializeField] private AidropSlotView slotView;
	[HideInInspector] public Trackable TrackableReference;

	public bool TimerStarted;

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

	public override void Initialize(AirdropDTO airdropDetailsDTO, int slotIndex, CardSlot cardSlot)
	{
		slotView.AirdropSlot = this;
		SlotID = slotIndex;

		Details = airdropDetailsDTO;
		IsEmpty = false;

		TrackableReference = TimeTracker.Instance.GetTrackable(SlotID);

		if (TrackableReference != null && TrackableReference.IsTracking)
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

	public override void Open()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
	}

	public void StartTracking()
	{
		TimerStarted = true;
		TimeTracker.Instance.StartTrackingTime(SlotID ,Details.UnlockDuration);
		slotView.InitialiseViewUIForUnlockingChest();
	}

	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().Initialize(new object[] { this });
	}

	public void UnlockChest()
	{
		slotView.InitialiseViewUIForUnlockedChest();
		TimerStarted = false;
	}

	public override void SetEmpty(int index)
	{
		IsEmpty = true;
		SlotID = index;
		TimerStarted = false;
		TrackableReference = null;
		Details = null;
		slotView.InitializeEmptyChestView();
	}
}
