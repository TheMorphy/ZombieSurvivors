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


public class CardOptions : MonoBehaviour
{
	[HideInInspector] public Card CardReference;

	[Space]
	[Header("Options Menu")]
	[SerializeField] private Button topActionButton;
	[SerializeField] private Button bottomActionButton;
	private TextMeshProUGUI topActionButtontext;
	private TextMeshProUGUI bottomActionButtontext;

	private Options topOption;
	private Options botOption;

	public void InitializeOptions(Card CardReference)
	{
		this.CardReference = CardReference;

		topActionButtontext = topActionButton.GetComponentInChildren<TextMeshProUGUI>();
		bottomActionButtontext = bottomActionButton.GetComponentInChildren<TextMeshProUGUI>();
		RefreshOptionsMenu();
	}

	public void ClearReference()
	{
		CardReference = null;
	}

	/// <summary>
	/// This just basically sets options menu button text values like in Clash Royale.
	/// </summary>
	public void RefreshOptionsMenu()
	{
		// If is in Active deck and is ready to upgrade
		if (CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Active)
		{
			topOption = Options.Upgrade;
			botOption = Options.Remove;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Active deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Active)
		{
			topOption = Options.Info;
			botOption = Options.Remove;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Inventory deck and is ready to upgrade
		else if (CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Inventory)
		{
			topOption = Options.Use;
			botOption = Options.Upgrade;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
		// If is in Inventory deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Inventory)
		{
			topOption = Options.Info;
			botOption = Options.Use;

			topActionButtontext.text = topOption.ToString();
			bottomActionButtontext.text = botOption.ToString();
		}
	}
	
	public void TopButtonAction()
	{
		switch (topOption)
		{
			case Options.Upgrade:
				CardReference.Upgrade();
				break;
			case Options.Use:
				CardReference.UseInActiveDeck();
				break;
			case Options.Remove:
				CardReference.UseInInventory();
				break;
			case Options.Info:
				CardReference.DisplayCardInfo();
				break;
		}
		Hide();
	}
	public void BotButtonAction()
	{
		switch (botOption)
		{
			case Options.Upgrade:
				CardReference.Upgrade();
				break;
			case Options.Use:
				CardReference.UseInActiveDeck();
				break;
			case Options.Remove:
				CardReference.UseInInventory();
				break;
			case Options.Info:
				CardReference.DisplayCardInfo();
				break;
		}
		Hide();
	}


	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void Show()
	{
		gameObject.SetActive(true);
		RefreshOptionsMenu();
	}

}
