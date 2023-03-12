using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum Options
{
	Upgrade,
	Use,
	Remove,
	Info
}

[RequireComponent(typeof(CardAnimation))]
public class CardOptions : MonoBehaviour
{
	[Serializable]
	public class ActionButtons
	{
		public Options buttonOption;
		public Sprite buttonSprite;
	}

	[HideInInspector] public Card CardReference;

	[Space]
	[Header("BUTTONS SETTINGS")]
	[SerializeField] private List<ActionButtons> buttons;

	[Space]
	[Header("GENRAL")]
	[SerializeField] private Button topActionButton;
	[SerializeField] private Button bottomActionButton;

	[SerializeField] private Image cardImage;
	[SerializeField] private TextMeshProUGUI cardLevel;

	private Options topOption;
	private Options botOption;

	[Header("ANIMATION")]
	private CardAnimation animation;
	private RectTransform rectTransform;

	public void InitializeOptions(Card CardReference)
	{
		animation = GetComponent<CardAnimation>();
		rectTransform = GetComponent<RectTransform>();

		this.CardReference = CardReference;
		RefreshOptionsMenu();
		Hide();
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
		cardImage.sprite = CardReference.CardSprite;
		cardLevel.text = CardReference.Details.CurrentCardLevel.ToString();

		// If is in Active deck and is ready to upgrade
		if (CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Active)
		{
			topOption = Options.Upgrade;
			botOption = Options.Remove;

			//topActionButton.image.sprite = upgradeSprite;
			//bottomActionButton.image.sprite = removeSprite;
		}
		// If is in Active deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Active)
		{
			topOption = Options.Info;
			botOption = Options.Remove;

			topActionButton.image.sprite = buttons.Find(x => x.buttonOption.Equals(topOption)).buttonSprite;
			bottomActionButton.image.sprite = buttons.Find(x => x.buttonOption.Equals(botOption)).buttonSprite;
		}
		// If is in Inventory deck and is ready to upgrade
		else if (CardReference.IsReadyToUpgrade == true && CardReference.CardSlot == CardSlot.Inventory)
		{
			topOption = Options.Use;
			botOption = Options.Upgrade;

			//topActionButton.image.sprite = useSprite;
			//bottomActionButton.image.sprite = upgradeSprite;
		}
		// If is in Inventory deck and is not ready to upgrade
		else if (CardReference.IsReadyToUpgrade == false && CardReference.CardSlot == CardSlot.Inventory)
		{
			topOption = Options.Info;
			botOption = Options.Use;

			//topActionButton.image.sprite = infoSprite;
			//bottomActionButton.image.sprite = useSprite;
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
		animation.PlayCloseAnimation(rectTransform, () => gameObject.SetActive(false));
	}

	public void Show()
	{
		gameObject.SetActive(true);
		// For the memes. Basically plays animation and calls the passed actions when it is completed
		animation.PlayShowAnimation(rectTransform, RefreshOptionsMenu);
	}
}
