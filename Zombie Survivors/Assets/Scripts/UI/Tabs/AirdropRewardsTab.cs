using System.Collections.Generic;
using System.Linq;
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
			List<CardDTO> newCards = (List<CardDTO>)args[1];

			openedAirdropImage.sprite = airdropDetailsDTO.AirdropSprite;

			var savedCards = SaveManager.ReadFromJSON<CardDTO>(Settings.ALL_CARDS);

			foreach(CardDTO newCard in newCards)
			{
				var savedCard = savedCards.FirstOrDefault(savedCard => savedCard.CardCode == newCard.CardCode);

				if (savedCard == null)
				{
					SaveManager.SaveToJSON(newCard, Settings.ALL_CARDS);
				}
				else
				{
					newCard.ID = SaveManager.GetNumSavedItems<CardDTO>(Settings.ALL_CARDS) + 1;
					savedCard.Ammount += newCard.Ammount;
					SaveManager.SaveToJSON(savedCard, Settings.ALL_CARDS);
				}
			}
			
			SaveManager.DeleteFromJSON<AirdropDTO>(airdropDetailsDTO.ID, Settings.AIRDROPS);

			CanvasManager.GetTab<EquipmentTab>().Initialize(null);

			for (int i = 0; i < newCards.Count; i++)
			{
				RewardCard rewardCard = Instantiate(rewardCardReference, rewardsGrid.transform).GetComponent<RewardCard>();
				rewardCard.gameObject.SetActive(true);
				rewardCard.DisplayRewardCard(newCards[i]);
			}
		}
	}
}
