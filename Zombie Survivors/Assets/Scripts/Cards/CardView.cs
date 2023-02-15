using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
	private Card cardReference;

	[Space]
	[Header("Options Menu")]
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private Button topActionButton;
	[SerializeField] private Button bottomActionButton;

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

	private void Awake()
	{
		cardReference = GetComponent<Card>();

		InitializeEmptyView();
		HideOptions();
	}

	private void Start()
	{
		cardButton.onClick.AddListener(() => {
			SelectCard(this);
		});
	}

	private void InitializeEmptyView()
	{
		cardSlotImage.sprite = emptySlotSprite;
		cardType.text = "";
		cardLevel.text = "";
		cardButton.enabled = false;
		cardLevelBar.transform.parent.gameObject.SetActive(false);
	}

	public void UpdateCardView(CardSO cardDetails)
	{
		cardButton.enabled = true;
		cardLevelBar.transform.parent.gameObject.SetActive(true);
		cardType.text = cardDetails.CardType.ToString();
		cardLevel.text = cardReference.CurrentCardLevel.ToString();
		cardRemainingLevel.text = cardReference.CardsInside.ToString() + " / " + cardReference.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = cardDetails.CardSprite;

		SetExpFillAmmount();
	}

	private void SelectCard(CardView clickedCardView)
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
			//foreach (CardView cardView in ActiveCardsController.ActiveDeck.Select(card => card.ca))
			//{
			//	if (cardView == clickedCardView)
			//	{
			//		cardView.DisplayOptions();
			//		_selectedCardView = cardView;
			//	}
			//	else
			//	{
			//		cardView.HideOptions();
			//	}
			//}
		}
	}

	public void DisplayOptions()
	{
		optionsMenu.gameObject.SetActive(true);

		topActionButton.onClick.AddListener(() => {
			ToggleOptions();
		});
	}

	public void HideOptions()
	{
		optionsMenu.gameObject.SetActive(false);
	}

	private void SetExpFillAmmount()
	{
		cardLevelBar.fillAmount = (float)cardReference.CardsInside / cardReference.CardsRequiredToNextLevel;
	}

	/// <summary>
	/// This just basically sets options menu button text values like in Clash Royale.
	/// </summary>
	private void ToggleOptions()
	{
		topActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Info";

		if (cardReference.IsReadyToUpgrade == true)
		{
			bottomActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade";
		}
		else
		{
			if (ActiveCardsController.ActiveDeck.Contains(cardReference.CardDetails)) // If this card is part of the Active deck set
			{
				bottomActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
			}
			else
			{
				bottomActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
			}
		}
	}
}
