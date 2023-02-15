using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward
{
	public CardSO RewardDetails;
	public int Ammount;
}

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
	private AirdropDTO airdropDetails;

	private List<Reward> rewards = new List<Reward>();
	private int rewardIndex = 0;

	List<CardSO> cardsToSave = new List<CardSO>();

	public override void Initialize()
	{
		
	}

	public override void InitializeWithArgs(object[] args)
	{
		airdropBtn.onClick.AddListener(() => {
			OpenChest();
		});

		slotReference = (Slot)args[0];
		airdropDetails = slotReference.AirdropDetails;

		AddCards();
		rewardIndex = rewards.Count;
		cardImage.gameObject.SetActive(true);
		airdropImage.sprite = airdropDetails.AirdropSprite;

		OpenChest();
	}


	public void OpenChest()
	{
		rewardIndex--;

		var cardToShow = rewards[rewardIndex];

		cardImage.sprite = cardToShow.RewardDetails.CardSprite;
		cardName.text = cardToShow.RewardDetails.CardType.ToString();
		cardRarity.text = cardToShow.RewardDetails.CardRarity.ToString();
		rewardAmmount.text = "x" + rewards[rewardIndex].Ammount.ToString();
		airopCountText.text = rewardIndex.ToString();

		if (rewardIndex == 0)
		{
			TimeTracker.Instance.ClearTime(slotReference.TrackingKey);
			PlayerPrefs.DeleteKey(airdropDetails.AirdropType + "_" + slotReference.SlotID);

			SaveManager.SaveToJSON(cardsToSave, Settings.ALL_CARDS_PATH);
			CanvasManager.Show<AirdropRewardsTab>(false, new object[] { airdropDetails, rewards });
		}
	}
	private void AddCards()
	{
		var commonCards = GameResources.Instance.CommonCards;
		var rareCards = GameResources.Instance.RareCards;
		var epicCards = GameResources.Instance.EpicCards;

		for (int i = 0; i < airdropDetails.CardAmmount; i++)
		{
			float randomChance = Random.value;
			CardSO cardToAdd = null;

			switch (airdropDetails.AirdropType)
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
		Reward cardReward = rewards.FirstOrDefault(x => x.RewardDetails.CardCode == cardToAdd.CardCode);

		if (cardReward != null)
		{
			cardReward.Ammount++;
		}
		else
		{
			cardsToSave.Add(cardToAdd);
			rewards.Add(new Reward()
			{
				RewardDetails = cardToAdd,
				Ammount = 1
			});
		}
	}
}
