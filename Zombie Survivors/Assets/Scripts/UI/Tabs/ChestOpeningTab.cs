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

	[HideInInspector] public Slot<AirdropDTO> SlotReference;
	[HideInInspector] public AirdropDTO AirdropDetailsDTO;

	[HideInInspector] public List<CardDTO> NewCards;
	private int newCardIndex = 0;

	public override void Initialize(object[] args)
	{
		if(args != null)
		{
			NewCards = new List<CardDTO>();

			SlotReference = (Slot<AirdropDTO>)args[0];

			AirdropDetailsDTO = SlotReference.Details;

			AddCards();
			cardImage.gameObject.SetActive(true);
			airdropImage.sprite = GameResources.Instance.GetAirdropSprite(AirdropDetailsDTO.AirdropType);

			newCardIndex = NewCards.Count;
			OpenChest();
		}
	}

	public void OpenChest()
	{
		newCardIndex--;

		if (newCardIndex == -1)
		{
			TimeTracker.Instance.ClearTime(SlotReference.SlotIndex);
			SlotReference.SetEmpty(SlotReference.SlotIndex);

			CanvasManager.Show<AirdropRewardsTab>(false, new object[] { AirdropDetailsDTO, NewCards });
			return;
		}

		cardImage.sprite = GameResources.Instance.GetCardSprite(NewCards[newCardIndex].CardType, NewCards[newCardIndex].CardRarity);
		cardName.text = NewCards[newCardIndex].CardType.ToString();
		cardRarity.text = NewCards[newCardIndex].CardRarity.ToString();
		rewardAmmount.text = "x" + NewCards[newCardIndex].Ammount.ToString();
		airopCountText.text = newCardIndex.ToString();

		
	}
	private void AddCards()
	{
		var commonCards = GameResources.Instance.CommonCards;
		var rareCards = GameResources.Instance.RareCards;
		var epicCards = GameResources.Instance.EpicCards;

		for (int i = 0; i < AirdropDetailsDTO.CardAmmount; i++)
		{
			float randomChance = Random.value;
			CardSO cardToAdd = null;

			switch (AirdropDetailsDTO.AirdropType)
			{
				case AirdropType.Wooden:
					if (randomChance > 0.0f && randomChance < 0.2f)
					{
						cardToAdd = commonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.2f && randomChance < 0.4f)
					{
						cardToAdd = rareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = epicCards[Random.Range(0, epicCards.Count)];
					}
					break;
				case AirdropType.Iron:
					if (randomChance > 0.0f && randomChance < 0.4f)
					{
						cardToAdd = commonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.4f && randomChance < 0.5f)
					{
						cardToAdd = rareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = epicCards[Random.Range(0, epicCards.Count)];
					}
					break;

				case AirdropType.Golden:
					if (randomChance > 0.0f && randomChance < 0.7f)
					{
						cardToAdd = commonCards[Random.Range(0, commonCards.Count)];
					}
					else if (randomChance > 0.7f && randomChance < 0.9f)
					{
						cardToAdd = rareCards[Random.Range(0, rareCards.Count)];
					}
					else
					{
						cardToAdd = epicCards[Random.Range(0, epicCards.Count)];
					}
					break;
			}
			SaveReward(cardToAdd);
		}
	}

	private void SaveReward(CardSO cardToAdd)
	{
		CardDTO newCard = NewCards.FirstOrDefault(x => x.Code == cardToAdd.CardCode);

		if (newCard != null)
		{
			newCard.Ammount++;
		}
		else
		{
			NewCards.Add(new CardDTO()
			{
				UpgradeAction = cardToAdd.UpgradeAction,
				WeaponStat = cardToAdd.WeaponStat,
				AmmoStat = cardToAdd.AmmoStat,
				PlayerStat = cardToAdd.PlayerStat,
				CardName = cardToAdd.CardName,
				CardRarity = cardToAdd.CardRarity,
				CardType = cardToAdd.CardType,
				CardSlot = Slot.Inventory,
				Code = cardToAdd.CardCode,
				ScallingConfiguration = cardToAdd.ScallingConfiguration,
				CurrentCardLevel = 1,
				CardsRequiredToNextLevel = 2,
				Ammount = 1
			});
		}
	}
}
