using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
	[SerializeField] private CardOptions cardOptions;

	[HideInInspector] public Card CardReference;
	
	[Space(5)]
	[Header("View Parameters")]
	[SerializeField] private Image cardSlotImage;
	[SerializeField] private Sprite emptySlotSprite;
	[SerializeField] private TextMeshProUGUI cardType;
    [SerializeField] private TextMeshProUGUI cardLevel;
    [SerializeField] private TextMeshProUGUI cardRemainingLevel;
    [SerializeField] private Image cardLevelBar;
    [SerializeField] private Button cardButton;

	public CardView _selectedCardView; // Keep track of the currently selected card

	public void InitializeEmptyView()
	{
		cardOptions.ClearReference();
		_selectedCardView = null;
		CardReference = null;
		cardSlotImage.sprite = emptySlotSprite;
		cardType.text = "";
		cardLevel.text = "";
		cardButton.enabled = false;
		cardLevelBar.transform.parent.gameObject.SetActive(false);
	}

	public void RefreshView()
	{
		cardType.text = CardReference.Details.CardName;
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.Details.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.Details.Ammount / CardReference.Details.CardsRequiredToNextLevel;
		if (CardReference.Details.Ammount >= CardReference.Details.CardsRequiredToNextLevel)
		{
			CardReference.IsReadyToUpgrade = true;
		}
		cardOptions.RefreshOptionsMenu();
	}

	public void InitializeCardView()
	{
		cardOptions.InitializeOptions(CardReference);
		cardButton.enabled = true;
		cardLevelBar.transform.parent.gameObject.SetActive(true);
		cardType.text = CardReference.Details.CardName;
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.Details.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.Details.Ammount / CardReference.Details.CardsRequiredToNextLevel;
		RefreshView();
	}

	public void SelectCard(CardView clickedCardView)
	{
		if (_selectedCardView == clickedCardView)
		{
			// If the clicked card is already selected, hide the options
			clickedCardView.cardOptions.Hide();
			_selectedCardView = null;
		}
		else
		{
			// Display options for the clicked card and hide options for all others
			foreach (CardView cardView in EquipmentTab.Cards.Select(card => card.CardView))
			{
				if (cardView == clickedCardView)
				{
					cardView.cardOptions.Show();
					_selectedCardView = cardView;
				}
				else
				{
					cardView.cardOptions.Hide();
				}
			}
		}
	}
}
