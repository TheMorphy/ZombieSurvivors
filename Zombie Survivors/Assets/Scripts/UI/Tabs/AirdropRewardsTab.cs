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
			AirdropDTO airdropDetailsDTO = (AirdropDTO)args[0];
			List<CardDTO> cardsDTOs = (List<CardDTO>)args[1];

			openedAirdropImage.sprite = airdropDetailsDTO.AirdropSprite;

			SaveManager.SaveToJSON<CardDTO>(cardsDTOs, Settings.ALL_CARDS);
			SaveManager.DeleteFromJSON<AirdropDTO>(airdropDetailsDTO.ID, Settings.AIRDROPS);


			for (int i = 0; i < cardsDTOs.Count; i++)
			{
				RewardCard rewardCard = Instantiate(rewardCardReference, rewardsGrid.transform).GetComponent<RewardCard>();
				rewardCard.gameObject.SetActive(true);
				rewardCard.DisplayRewardCard(cardsDTOs[i]);
			}
		}
	}
}
