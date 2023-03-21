using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;
	[SerializeField] private CharacterSelector characterSelector;

	public static List<Card> InitializedCards;

	public override void Initialize(object[] args)
	{
		InitializedCards = new List<Card>();

		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS);

		var activeCards = allCards.Where(x => x.CardSlot == Slot.Active).ToList();
		var inventoryCards = allCards.Where(x => x.CardSlot == Slot.Inventory).ToList();

		activeCardsController.InitializeSlots(activeCards, Slot.Active);
		inventoryController.InitializeSlots(inventoryCards, Slot.Inventory);

		characterSelector.InitializeChatacter();
	}

	public static void AddInitializedCard(Card card)
	{
		var toCheck = InitializedCards.FirstOrDefault(x => x.Details?.Code == card.Details.Code);
		if (toCheck == null)
		{
			InitializedCards.Add(card);
		}
		else
		{
			InitializedCards.Remove(toCheck);
			InitializedCards.Add(card);
		}
	}

	public void UpdateInventory()
	{
		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS);

		foreach (var card in allCards)
		{
			var cardInList = InitializedCards.Where(x => x.Details != null).FirstOrDefault(x => x.Details.Code.Equals(card.Code));

			if (cardInList == null)
			{
				inventoryController.InitializeSlot(card, Slot.Inventory);
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
