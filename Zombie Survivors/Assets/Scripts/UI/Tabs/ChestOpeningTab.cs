using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestOpeningTab : Tab
{
	[Header("Airdrop")]
	[SerializeField] private Image airdropImage;
	[SerializeField] private TextMeshProUGUI airopCountText;
	[SerializeField] private Button airdropBtn;

	[Space]
	[Header("Card")]
	[SerializeField] private Image cardImage;
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI cardRarity;
	[SerializeField] private TextMeshProUGUI rewardAmmount;

	private Slot slotReference;
	private AirdropDTO airdropDetailsDTO;

	private List<CardDTO> cardsDTOs = new List<CardDTO>();
	private int cardIndex = 0;

	public override void Initialize()
	{
		
	}

	public override void InitializeWithArgs(object[] args)
	{
		airdropBtn.onClick.AddListener(() => {
			OpenChest();
		});

		slotReference = (Slot)args[0];
		airdropDetailsDTO = slotReference.AirdropDetails;

		AddCards();
		cardIndex = cardsDTOs.Count;
		cardImage.gameObject.SetActive(true);
		airdropImage.sprite = airdropDetailsDTO.AirdropSprite;

		OpenChest();
	}


	public void OpenChest()
	{
		cardIndex--;

		cardImage.sprite = cardsDTOs[cardIndex].CardSprite;
		cardName.text = cardsDTOs[cardIndex].CardType.ToString();
		cardRarity.text = cardsDTOs[cardIndex].CardRarity.ToString();
		rewardAmmount.text = "x" + cardsDTOs[cardIndex].Ammount.ToString();
		airopCountText.text = cardIndex.ToString();

		if (cardIndex == 0)
		{
			TimeTracker.Instance.ClearTime(slotReference.TrackingKey);

			SaveManager.SaveToJSON(cardsDTOs, Settings.ALL_CARDS_PATH);
			SaveManager.DeleteFromJSON(airdropDetailsDTO, Settings.AIRDROPS_PATH);

			CanvasManager.Show<AirdropRewardsTab>(false, new object[] { airdropDetailsDTO, cardsDTOs });
		}
	}
	private void AddCards()
	{
		var commonCards = GameResources.Instance.CommonCards;
		var rareCards = GameResources.Instance.RareCards;
		var epicCards = GameResources.Instance.EpicCards;

		for (int i = 0; i < airdropDetailsDTO.CardAmmount; i++)
		{
			float randomChance = Random.value;
			CardSO cardToAdd = null;

			switch (airdropDetailsDTO.AirdropType)
			{
				case AirdropType.Wooden:
					if (randomChance > 0.0f && randomChance < 0.2f)
					{
						cardToAdd = GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.2f && randomChance < 0.4f)
					{
						cardToAdd = GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)];
					}
					break;
				case AirdropType.Iron:
					if (randomChance > 0.0f && randomChance < 0.4f)
					{
						cardToAdd = GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.4f && randomChance < 0.5f)
					{
						cardToAdd = GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)];
					}
					break;

				case AirdropType.Gold:
					if (randomChance > 0.0f && randomChance < 0.7f)
					{
						cardToAdd = GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.7f && randomChance < 0.9f)
					{
						cardToAdd = GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)];
					}
					break;
			}
			SaveReward(cardToAdd);
		}
	}

	private void SaveReward(CardSO cardToAdd)
	{
		CardDTO cardReward = cardsDTOs.FirstOrDefault(x => x.CardCode == cardToAdd.CardCode);

		if (cardReward != null)
		{
			cardReward.Ammount++;
		}
		else
		{
			cardsDTOs.Add(new CardDTO()
			{
				UpgradeAction = cardToAdd.UpgradeAction,
				ScallingConfiguration = cardToAdd.ScallingConfiguration,
				CardSprite = cardToAdd.CardSprite,
				UpgradeStat = cardToAdd.UpgradeStat,
				CardCode = cardToAdd.CardCode,
				CardName = cardToAdd.CardName,
				CardRarity = cardToAdd.CardRarity,
				CardType = cardToAdd.CardType,
				UpgradeValue = cardToAdd.UpgradeValue,
				Ammount = 1
			});
		}
	}
}
