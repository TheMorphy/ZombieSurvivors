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
	public int SlotID;

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
		IsEmpty = false;
		SlotID = slotIndex;

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

	public void OpenChest()
	{
		CanvasManager.Show<ChestOpeningTab>(true, new object[] { this });
	}

	public void SetEmptySlot()
	{
		IsEmpty = true;
		TimerStarted = false;
		SlotID = 0;
		TrackableReference = null;
		AirdropDetailsDTO = null;
		slotView.InitializeEmptyChestView();
	}
}
