using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsController : MonoBehaviour
{
	[Space]
	[Header("OPENING WINDOW")]
	[SerializeField] private GameObject openingWindow;
    [SerializeField] private Image chestImage;
    [SerializeField] private TextMeshProUGUI chestAmmountText;
    [SerializeField] private Button chestBtn;

	[Space]
	[Header("CARD")]
	[SerializeField] private Image cardImage;
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI cardRarity;
	[SerializeField] private TextMeshProUGUI rewardAmmount;

	[Space]
	[Header("DISPLAY REWARDS")]
	[SerializeField] private GameObject rewardsWindow;
	[SerializeField] private GameObject rewardsGrid;
	[SerializeField] private Image openedhestImage;
	[SerializeField] private GameObject rewardCardReference;

	private AirdropDetails airdropDetails;

	private List<CardSO> cards = new List<CardSO>();

	int cardsToOpenCount = 0;

	private void Start()
	{
		rewardsWindow.SetActive(false);
		openingWindow.SetActive(true);
	}

	public void InitializeWindow(AirdropDetails airdrop)
	{
		Show();

		airdropDetails = airdrop;
		cardImage.gameObject.SetActive(true);
		chestImage.sprite = airdropDetails.AirdropSprite;
		chestAmmountText.text = airdropDetails.CardAmmount.ToString();

		AddCards();

		OpenChest();
	}

	public void OpenChest()
	{
		cardsToOpenCount--;

		if (cardsToOpenCount == 0)
		{
			openingWindow.SetActive(false);
			rewardsWindow.SetActive(true);

			openedhestImage.sprite = airdropDetails.AirdropSprite;

			for (int i = 0; i < airdropDetails.CardAmmount; i++)
			{
				RewardCard rewardCard = Instantiate(rewardCardReference, rewardsGrid.transform).GetComponent<RewardCard>();
				rewardCard.gameObject.SetActive(true);
				rewardCard.InitializeRewardCard(cards[i].CardSprite, Random.Range(1, 5)); // For now random number of cards
			}
			return;
		}
			

		var cardToShow = cards[cardsToOpenCount];

		cardImage.sprite = cardToShow.CardSprite;
		cardName.text = cardToShow.CardType.ToString();
		cardRarity.text = cardToShow.CardRarity.ToString();
		rewardAmmount.text = "+" + cardToShow.UpgradeAmmount.ToString();

		chestAmmountText.text = cardsToOpenCount.ToString();
	}

	private void AddCards()
	{
		var commonCards = GameResources.Instance.CoomonCards;
		var rareCards = GameResources.Instance.RareCards;

		cardsToOpenCount = airdropDetails.CardAmmount;

		for (int i = 0; i < airdropDetails.CardAmmount; i++)
		{
			float randomChance = Random.value;

			switch (airdropDetails.AirdropType)
			{
				case AirdropType.Wooden:

					if (randomChance > 0.0f && randomChance < 0.2f)
					{
						cards.Add(GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.CoomonCards[Random.Range(0, commonCards.Count)]);
					}
					break;
				case AirdropType.Iron:
					if (randomChance > 0.0f && randomChance < 0.3f)
					{
						cards.Add(GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.CoomonCards[Random.Range(0, commonCards.Count)]);
					}
					break;

				case AirdropType.Gold:
					if (randomChance > 0.0f && randomChance < 0.6f)
					{
						cards.Add(GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.CoomonCards[Random.Range(0, commonCards.Count)]);
					}
					break;
			}
		}
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
	public void Show()
	{
		gameObject.SetActive(true);
	}
}
