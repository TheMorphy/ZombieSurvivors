using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTab : Tab
{
	[SerializeField] private SkipWaitingTab skipWaitingTab;

	[Header("Controllers")]
	[SerializeField] private CollectedAirdropsController slotsController;

	public override void Initialize()
	{
		skipWaitingTab.Hide();
		InitializeCollectedAirdropSlots();
	}

	public override void InitializeWithArgs(object[] args)
	{

	}

	private void InitializeCollectedAirdropSlots()
	{
		List<AirdropDTO> collectedAirdrops = SaveManager.ReadFromJSON<AirdropDTO>(Settings.AIRDROPS_PATH);

		if(collectedAirdrops.Count > 0 ) 
		{
			foreach (var collectedAirdrop in collectedAirdrops)
			{
				slotsController.InitializeAirdropSlotIfEmpty(collectedAirdrop);
			}
		}

		slotsController.GetSlots().ForEach((slot) =>
		{
			if (slot.IsEmpty)
			{
				slot.SetEmptySlot();
			}
		});
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void ResetPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public CollectedAirdropsController GetSlotsController()
	{
		return slotsController;
	}

	public SkipWaitingTab GetSkipWaitingTab()
	{
		return skipWaitingTab;
	}

	private void OnDisable()
	{
		for (int i = 0; i < slotsController.GetSlots().Count; i++)
		{
			Slot slot = slotsController.GetSlot(i);
			if (slot.IsEmpty == false && slot.TimerStarted)
			{
				TimeTracker.Instance.StartTrackingTime(slot);
			}
		}
	}
}
