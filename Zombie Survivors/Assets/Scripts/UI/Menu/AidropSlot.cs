using UnityEngine;

[RequireComponent(typeof(AidropSlotView))]
public class AidropSlot : Slot<AirdropDTO>
{
	[SerializeField] private AidropSlotView slotView;
	[HideInInspector] public Trackable TrackableReference;
	[HideInInspector] public AirdropDTO AirdropDetailsDTO;

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

	public override void Initialize(AirdropDTO airdropDetailsDTO, int slotIndex)
	{
		slotView.AirdropSlot = this;
		SlotID = slotIndex;

		AirdropDetailsDTO = airdropDetailsDTO;
		IsEmpty = false;

		TrackableReference = TimeTracker.Instance.GetTrackable(SlotID);

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

	public override void Open()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
	}

	public void StartTracking()
	{
		TimerStarted = true;
		TrackableReference = TimeTracker.Instance.SetNewStrackable(SlotID, AirdropDetailsDTO.UnlockDuration);
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

	public override void SetEmpty()
	{
		IsEmpty = true;
		TimerStarted = false;
		TrackableReference = null;
		AirdropDetailsDTO = null;
		slotView.InitializeEmptyChestView();
	}
}
