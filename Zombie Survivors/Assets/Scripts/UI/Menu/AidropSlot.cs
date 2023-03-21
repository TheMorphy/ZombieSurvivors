using UnityEngine;

[RequireComponent(typeof(AidropSlotView))]
public class AidropSlot : Slot<AirdropDTO>
{
	public AidropSlotView SlotView;

	[HideInInspector] public Trackable TrackableReference;
	[HideInInspector] public Sprite AirdropSprite;
	[HideInInspector] public bool TimerStarted;
	
	private void Update()
	{
		// I don't like this approach, but it works for now. This basically
		if (TrackableReference != null && TimerStarted)
		{
			if (TrackableReference.RemainingSeconds <= 0)
			{
				UnlockChest();
			}
			else
			{
				SlotView.unlockTimeText.text = TrackableReference.TimerText;
			}
		}
	}

	public override void Initialize(AirdropDTO airdropDetailsDTO, int slotIndex, Slot cardSlot)
	{
		SlotView.AirdropSlot = this;

		SlotIndex = slotIndex;
		Details = airdropDetailsDTO;
		IsEmpty = false;

		AirdropSprite = GameResources.Instance.GetAirdropSprite(airdropDetailsDTO.AirdropType);

		TrackableReference = TimeTracker.Instance.GetTrackable(SlotIndex);

		if (TrackableReference != null && TrackableReference.IsTracking)
		{
			TimerStarted = true;
			SlotView.InitialiseViewUIForUnlockingChest();
		}
		else
		{
			TimerStarted = false;
			SlotView.InitialiseViewUIForLockedChest();
		}
	}

	public override void Open()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
	}

	/// <summary>
	/// Saves slot ID in TimeTracker and starts tracking the opening time.
	/// </summary>
	public void StartTracking()
	{
		TimerStarted = true;
		TimeTracker.Instance.StartTrackingTime(SlotIndex ,Details.UnlockDuration);
		SlotView.InitialiseViewUIForUnlockingChest();
	}

	/// <summary>
	/// Passes the slot reference to the KkipWaitingTab to know, for which slot to skip waiting time.
	/// </summary>
	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().Initialize(new object[] { this });
	}

	/// <summary>
	/// Sets the chest available to be opened
	/// </summary>
	public void UnlockChest()
	{
		SlotView.InitialiseViewUIForUnlockedChest();
		TimerStarted = false;
	}

	/// <summary>
	/// Clears slot references and other values. 
	/// </summary>
	public override void SetEmpty(int index)
	{
		IsEmpty = true;
		SlotIndex = index;
		TimerStarted = false;
		TrackableReference = null;
		Details = null;
		SlotView.InitializeEmptyChestView();
	}
}
