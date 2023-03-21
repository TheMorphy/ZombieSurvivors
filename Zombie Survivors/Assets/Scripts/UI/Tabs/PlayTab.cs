using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTab : Tab
{
	[SerializeField] private SkipWaitingTab skipWaitingTab;

	[Header("Controllers")]
	[SerializeField] private CollectedAirdropsController airdropsSlotsController;

	public override void Initialize(object[] args)
	{
		skipWaitingTab.Hide();
		InitializeCollectedAirdropSlots();

		// Initializes time tracker for each slot if not already set up
		CreateSlotsTimeTrackers();
	}

	private void InitializeCollectedAirdropSlots()
	{
		List<AirdropDTO> collectedAirdrops = SaveManager.ReadFromJSON<AirdropDTO>(Settings.AIRDROPS);

		airdropsSlotsController.InitializeSlots(collectedAirdrops, Slot.None);
	}

	private void CreateSlotsTimeTrackers()
	{
		if (TimeTracker.Instance.Trackables.Count != CollectedAirdropsController.MAX_SLOT_COUNT)
		{
			airdropsSlotsController.GetSlots().ForEach(slot => {
				TimeTracker.Instance.InitializeTrackable(slot.SlotIndex);
			});

		}
	}

	public void StartGame()
	{
		AudioManager.Instance.PlayMusicWithFade(SoundTitle.Gameplay_Theme, 0.8f);

		SceneManager.LoadScene(1);
	}

	public void ResetPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public CollectedAirdropsController GetSlotsController()
	{
		return airdropsSlotsController;
	}

	public SkipWaitingTab GetSkipWaitingTab()
	{
		return skipWaitingTab;
	}
}
