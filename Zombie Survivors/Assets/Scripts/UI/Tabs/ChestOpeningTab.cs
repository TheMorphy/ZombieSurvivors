using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.LookDev;
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

	private Slot<AirdropDTO> slotReference;
	private AirdropDTO airdropDetailsDTO;

	public List<CardDTO> newCards;
	private int newCardIndex = 0;

	public override void Initialize(object[] args)
	{
		if(args != null)
		{
			newCards = new List<CardDTO>();

			slotReference = (Slot<AirdropDTO>)args[0];

			airdropBtn.onClick.AddListener(() =>
			{
				OpenChest();
			});

			airdropDetailsDTO = slotReference.Details;

			AddCards();
			cardImage.gameObject.SetActive(true);
			airdropImage.sprite = airdropDetailsDTO.AirdropSprite;

			newCardIndex = newCards.Count;
			OpenChest();
		}
	}

	public void OpenChest()
	{
		newCardIndex--;

		cardImage.sprite = newCards[newCardIndex].CardSprite;
		cardName.text = newCards[newCardIndex].CardType.ToString();
		cardRarity.text = newCards[newCardIndex].CardRarity.ToString();
		rewardAmmount.text = "x" + newCards[newCardIndex].Ammount.ToString();
		airopCountText.text = newCardIndex.ToString();

		if (newCardIndex == 0)
		{
			TimeTracker.Instance.ClearTime(slotReference.SlotID);
			slotReference.SetEmpty(slotReference.SlotID);

			CanvasManager.Show<AirdropRewardsTab>(false, new object[] { airdropDetailsDTO, newCards });
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
		CardDTO newCard = newCards.FirstOrDefault(x => x.CardCode == cardToAdd.CardCode);

		if (newCard != null)
		{
			newCard.Ammount++;
		}
		else
		{
			newCards.Add(new CardDTO()
			{
				UpgradeAction = cardToAdd.UpgradeAction,
				CardSprite = cardToAdd.CardSprite,
				WeaponStat = cardToAdd.WeaponStat,
				AmmoStat = cardToAdd.AmmoStat,
				PlayerStat = cardToAdd.PlayerStat,
				CardCode = cardToAdd.CardCode,
				CardName = cardToAdd.CardName,
				CardRarity = cardToAdd.CardRarity,
				CardType = cardToAdd.CardType,

				ScallingConfiguration = cardToAdd.ScallingConfiguration,
				CurrentCardLevel = 1,
				CardsRequiredToNextLevel = 2,
				Ammount = 1
			});
		}
	}
}
