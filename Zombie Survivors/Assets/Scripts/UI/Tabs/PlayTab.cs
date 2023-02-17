using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTab : Tab
{
	[SerializeField] private SkipWaitingTab skipWaitingTab;

	[Header("Controllers")]
	[SerializeField] private CollectedAirdropsController slotsController;

	public override void Initialize(object[] args)
	{
		skipWaitingTab.Hide();
		InitializeCollectedAirdropSlots();
	}

	private void InitializeCollectedAirdropSlots()
	{
		List<AirdropDTO> collectedAirdrops = SaveManager.ReadFromJSON<AirdropDTO>(Settings.AIRDROPS);

		if(collectedAirdrops.Count > 0)
		{
			foreach (var item in collectedAirdrops)
			{
				slotsController.InitializeSlot(item);
			}
	
		}

		slotsController.GetSlots().ForEach((slot) =>
		{
			if (slot.IsEmpty)
			{
				slot.SetEmpty();
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
}
