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

	public bool IsInOptions = false;

	private void Awake()
	{
		cardReference = GetComponent<Card>();

		InitializeEmptyView();
		HideOptions();
	}

	private void Start()
	{
		cardButton.onClick.AddListener(() => {
			SelectCard();
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

	private void SelectCard()
	{
		if (IsInOptions)
		{
			IsInOptions = false;
			HideOptions();
		}
		else
		{
			IsInOptions = true;
			DisplayOptions();
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
		switch (cardReference.cardSlot)
		{
			case CardSlot.Active:

				if (cardReference.IsReadyToUpgrade == false)
				{
					topActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Info";
					// TODO: Show Card Info
				}
				else
				{
					topActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade";
					// TODO: Call Upgrade Card Method
				}

				break;

			case CardSlot.Inventory:

				if (cardReference.IsReadyToUpgrade == false)
				{
					topActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Info";
					// TODO: Show Card Info
				}
				else
				{
					topActionButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
					// TODO: Add Card to active cards slot or replace if there are no empty
				}

				break;
		}
	}
}
