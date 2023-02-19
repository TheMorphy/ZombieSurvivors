using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirdropRewardsTab : Tab
{
	[SerializeField] private GameObject rewardsGrid;
	[SerializeField] private Image openedAirdropImage;
	[SerializeField] private GameObject rewardCardReference;

	private void Update()
	{
		if(rewardsGrid.transform.childCount > 0 && Input.GetMouseButtonDown(0))
		{
			CanvasManager.ShowLast();
		}
	}

	public override void Initialize(object[] args)
	{
		if(args != null)
		{
			for (int i = 0; i < rewardsGrid.transform.childCount; i++)
			{
				Destroy(rewardsGrid.transform.GetChild(i).gameObject);
			}

			AirdropDTO airdropDetailsDTO = (AirdropDTO)args[0];
			SaveManager.DeleteFromJSON(airdropDetailsDTO, Settings.AIRDROPS);

			List<CardDTO> newCards = (List<CardDTO>)args[1];
			SaveManager.SaveToJSON(newCards, Settings.CARDS);

			openedAirdropImage.sprite = airdropDetailsDTO.AirdropSprite;

			CanvasManager.GetTab<EquipmentTab>().Initialize(new object[] { newCards });

			for (int i = 0; i < newCards.Count; i++)
			{
				RewardCard rewardCard = Instantiate(rewardCardReference, rewardsGrid.transform).GetComponent<RewardCard>();
				rewardCard.gameObject.SetActive(true);
				rewardCard.DisplayRewardCard(newCards[i]);
			}
		}
	}
}
