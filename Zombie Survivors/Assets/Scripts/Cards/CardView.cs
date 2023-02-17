using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Options
{
	Upgrade,
	Use,
	Remove,
	Info
}

public class CardView : MonoBehaviour
{
	[HideInInspector] public Card CardReference;

	[Space]
	[Header("Options Menu")]
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private Button topActionButton;
	[SerializeField] private Button bottomActionButton;
	private Options topOption = Options.Use;
	private Options botOption = Options.Remove;

	[Space(5)]
	[Header("View Parameters")]
	[SerializeField] private Image cardSlotImage;
	[SerializeField] private Sprite emptySlotSprite;
	[SerializeField] private TextMeshProUGUI cardType;
    [SerializeField] private TextMeshProUGUI cardLevel;
    [SerializeField] private TextMeshProUGUI cardRemainingLevel;
    [SerializeField] private Image cardLevelBar;
    [SerializeField] private Button cardButton;

	private CardView _selectedCardView; // Keep track of the currently selected card

	private void Start()
	{
		cardButton.onClick.AddListener(() => {
			SelectCard(this);
		});
	}

	public void InitializeEmptyView()
	{
		cardSlotImage.sprite = emptySlotSprite;
		cardType.text = "";
		cardLevel.text = "";
		cardButton.enabled = false;
		cardLevelBar.transform.parent.gameObject.SetActive(false);
	}

	public void UpdateCardView()
	{
		cardButton.enabled = true;
		cardLevelBar.transform.parent.gameObject.SetActive(true);
		cardType.text = CardReference.CardDetails.CardType.ToString();
		cardLevel.text = CardReference.CardDetails.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.CardDetails.Ammount.ToString() + " / " + CardReference.CardDetails.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.CardDetails.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.CardDetails.Ammount / CardReference.CardDetails.CardsRequiredToNextLevel;
	}

	public void SelectCard(CardView clickedCardView)
	{
		if (_selectedCardView == clickedCardView)
		{
			// If the clicked card is already selected, hide the options
			clickedCardView.HideOptions();
			_selectedCardView = null;
		}
		else
		{
			// Display options for the clicked card and hide options for all others
			foreach (CardView cardView in ActiveCardsController.ActiveDeck.Select(card => card.CardView))
			{
				if (cardView == clickedCardView)
				{
					cardView.DisplayOptions();
					_selectedCardView = cardView;
				}
				else
				{
					cardView.HideOptions();
				}
			}
		}
	}

	public void DisplayOptions()
	{
		optionsMenu.gameObject.SetActive(true);
		ShowToggleOptions();

		topActionButton.onClick.AddListener(() => {
			DoAction(topOption);
		});
		bottomActionButton.onClick.AddListener(() => {
			DoAction(botOption);
		});
	}

	public void HideOptions()
	{
		optionsMenu.gameObject.SetActive(false);
	}

	private void DoAction(Options currentOption)
	{
		switch (currentOption)
		{
			case Options.Upgrade:
				CardReference.Upgrade();
				break;
			case Options.Use:
				CardReference.UseInActiveDeck();
				break;
			case Options.Remove:
				CardReference.RemoveFromActiveDeck();
				break;
			case Options.Info:
				CardReference.DisplayCardInfo();
				break;
		}
	}

	/// <summary>
	/// This just basically sets options menu button text values like in Clash Royale.
	/// </summary>
	private void ShowToggleOptions()
	{
		// If is in Active deck and is ready to upgrade
		if (CardReference.IsReadyToUpgrade == true && ActiveCardsController.ActiveDeck.Contains(CardReference))
		{
			topOption = Options.Info;
			botOption = Options.Upgrade;

			topActionButton.GetComponentInChildren<TextMeshProUGUI>().text = topOption.ToString();
			bottomActionButton.GetComponentInChildren<TextMeshProUGUI>().text = botOption.ToString();
			

		}
		// If is in Active deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && ActiveCardsController.ActiveDeck.Contains(CardReference))
		{
			topOption = Options.Info;
			botOption = Options.Remove;

			topActionButton.GetComponentInChildren<TextMeshProUGUI>().text = topOption.ToString();
			bottomActionButton.GetComponentInChildren<TextMeshProUGUI>().text = botOption.ToString();
		}
		// If is in Inventory deck and is ready to upgrade
		else if(CardReference.IsReadyToUpgrade == true && ActiveCardsController.ActiveDeck.Contains(CardReference) == false)
		{
			topOption = Options.Use;
			botOption = Options.Upgrade;

			topActionButton.GetComponentInChildren<TextMeshProUGUI>().text = topOption.ToString();
			bottomActionButton.GetComponentInChildren<TextMeshProUGUI>().text = botOption.ToString();
		}
		// If is in Inventory deck and is not ready to upgrade
		else
		{
			topOption = Options.Info;
			botOption = Options.Use;

			topActionButton.GetComponentInChildren<TextMeshProUGUI>().text = topOption.ToString();
			bottomActionButton.GetComponentInChildren<TextMeshProUGUI>().text = botOption.ToString();
		}
	}
}
