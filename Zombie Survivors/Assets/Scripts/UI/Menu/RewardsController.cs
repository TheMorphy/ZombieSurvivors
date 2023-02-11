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

	private int cardsToOpenCount = 0;

	private bool rewardsShowed = false;

	private void Start()
	{
		rewardsWindow.SetActive(false);
		openingWindow.SetActive(true);
	}

	private void Update()
	{
		if(Input.GetMouseButtonDown(0) && rewardsShowed)
		{
			MainMenuViewController.Instance.GetSlotsController().Show();
			MainMenuViewController.Instance.EnableNavigationTab();
			Hide();
		}
	}

	public void InitializeWindow(AirdropDetails airdrop)
	{
		MainMenuViewController.Instance.GetSlotsController().Hide();
		MainMenuViewController.Instance.DisableNavigationTab();

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
				rewardCard.InitializeReward(cards[i], Random.Range(1, 5)); // For now random number of cards
			}
			rewardsShowed = true;
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
		var commonCards = GameResources.Instance.CommonCards;
		var rareCards = GameResources.Instance.RareCards;
		var epicCards = GameResources.Instance.EpicCards;

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
					else if (randomChance > 0.2f && randomChance < 0.4f)
					{
						cards.Add(GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)]);
					}
					break;
				case AirdropType.Iron:
					if (randomChance > 0.0f && randomChance < 0.1f)
					{
						cards.Add(GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)]);
					}
					else if (randomChance > 0.1f && randomChance < 0.2f)
					{
						cards.Add(GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)]);
					}
					break;

				case AirdropType.Gold:
					if (randomChance > 0.0f && randomChance < 0.3f)
					{
						cards.Add(GameResources.Instance.RareCards[Random.Range(0, rareCards.Count)]);
					}
					else if(randomChance > 0.3f && randomChance < 0.9f)
					{
						cards.Add(GameResources.Instance.CommonCards[Random.Range(0, commonCards.Count)]);
					}
					else
					{
						cards.Add(GameResources.Instance.EpicCards[Random.Range(0, epicCards.Count)]);
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
