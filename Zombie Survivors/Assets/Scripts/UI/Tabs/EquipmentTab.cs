using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;
	[SerializeField] private CharacterSelector characterSelector;

	[HideInInspector] public static List<Card> Cards;

	public override void Initialize(object[] args)
	{
		Cards = new List<Card>();

		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS);

		var activeCards = allCards.Where(x => x.CardSlot == CardSlot.Active).ToList();
		var inventoryCards = allCards.Where(x => x.CardSlot == CardSlot.Inventory).ToList();

		activeCardsController.InitializeSlots(activeCards, CardSlot.Active);
		inventoryController.InitializeSlots(inventoryCards, CardSlot.Inventory);

		characterSelector.InitializeChatacter();
	}

	public static void Add(Card card)
	{
		if (!Cards.Contains(card))
		{
			Cards.Add(card);
		}
	}

	public void UpdateInventory()
	{
		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS);

		foreach (var card in allCards)
		{
			var cardInList = Cards.FirstOrDefault(x => x.Details.Code.Equals(card.Code));

			if (cardInList == null)
			{
				inventoryController.InitializeSlot(card, CardSlot.Inventory);
			}
			else
			{
				cardInList.Details = card;
				cardInList.CardView.RefreshView();
			}
		}
	}

	public InventoryController GetInventory()
	{
		return inventoryController;
	}

	public ActiveCardsController GetActiveCardsController()
	{
		return activeCardsController;
	}
}
