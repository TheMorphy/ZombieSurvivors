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
    [SerializeField] private TextMeshProUGUI cardLevel;
    [SerializeField] private TextMeshProUGUI cardRemainingLevel;
    [SerializeField] private Image cardLevelBar;
    [SerializeField] private Button cardButton;

	private CardView _selectedCardView; // Keep track of the currently selected card

	public void InitializeEmptyView()
	{
		cardOptions.ClearReference();
		_selectedCardView = null;
		CardReference = null;
		cardSlotImage.sprite = emptySlotSprite;
		cardLevel.text = "";
		cardButton.enabled = false;
		cardLevelBar.transform.parent.gameObject.SetActive(false);
	}

	public void RefreshView()
	{
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.CardSprite;
		cardLevelBar.fillAmount = (float)CardReference.Details.Ammount / CardReference.Details.CardsRequiredToNextLevel;
		if (CardReference.Details.Ammount >= CardReference.Details.CardsRequiredToNextLevel)
		{
			CardReference.IsReadyToUpgrade = true;
		}
		else
		{
			CardReference.IsReadyToUpgrade = false;
		}
		cardOptions.RefreshOptionsMenu();
	}

	public void InitializeCardView()
	{
		cardOptions.InitializeOptions(CardReference);
		cardButton.enabled = true;
		cardLevelBar.transform.parent.gameObject.SetActive(true);
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();
		cardRemainingLevel.text = CardReference.Details.Ammount.ToString() + " / " + CardReference.Details.CardsRequiredToNextLevel.ToString();
		cardSlotImage.sprite = CardReference.CardSprite;
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
					AudioManager.Instance.PlaySFX(SoundTitle.Card_Select);
					CardReference.CardAnimation.PlayAnimation();
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
