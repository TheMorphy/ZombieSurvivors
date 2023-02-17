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
	private TextMeshProUGUI topActionButtontext;
	private TextMeshProUGUI bottomActionButtontext;

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

	public void InitializeEmptyView()
	{
		InitializeOptionsMenu();
		cardSlotImage.sprite = emptySlotSprite;
		cardType.text = "";
		cardLevel.text = "";
		cardButton.enabled = false;
		cardLevelBar.transform.parent.gameObject.SetActive(false);
	}

	public void RefreshView()
	{
		RefreshOptionsMenu();
		cardType.text = CardReference.Details.CardType.ToString();
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.Details.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.Details.Ammount / CardReference.Details.CardsRequiredToNextLevel;
	}

	public void InitializeCardView()
	{
		InitializeOptionsMenu();
		cardButton.enabled = true;
		cardLevelBar.transform.parent.gameObject.SetActive(true);
		cardType.text = CardReference.Details.CardType.ToString();
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.Details.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.Details.Ammount / CardReference.Details.CardsRequiredToNextLevel;

		topActionButton.onClick.AddListener(() => {
			DoAction(topOption);
		});

		bottomActionButton.onClick.AddListener(() => {
			DoAction(botOption);
		});

		cardButton.onClick.AddListener(() => {
			SelectCard(this);
		});
	}

	private void InitializeOptionsMenu()
	{
		optionsMenu.SetActive(true);
		topActionButtontext = topActionButton.GetComponentInChildren<TextMeshProUGUI>();
		bottomActionButtontext = bottomActionButton.GetComponentInChildren<TextMeshProUGUI>();
		RefreshOptionsMenu();
		optionsMenu.SetActive(false);
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
			foreach (CardView cardView in EquipmentTab.Cards.Where(x => !x.IsEmpty).Select(card => card.CardView))
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

	private void DisplayOptions()
	{
		optionsMenu.gameObject.SetActive(true);
		RefreshOptionsMenu();
	}

	private void HideOptions()
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
	private void RefreshOptionsMenu()
	{
		// If is in Active deck and is ready to upgrade
		if (CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Active)
		{
			print("1");
			topOption = Options.Upgrade;
			botOption = Options.Remove;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Active deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Active)
		{
			print("2");
			topOption = Options.Info;
			botOption = Options.Remove;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Inventory deck and is ready to upgrade
		else if(CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Inventory)
		{
			print("3");
			topOption = Options.Use;
			botOption = Options.Upgrade;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Inventory deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Inventory)
		{
			print("4");
			topOption = Options.Info;
			botOption = Options.Use;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
	}
}
