using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;

	[HideInInspector] public static HashSet<Card> Cards = new HashSet<Card>();

	public override void Initialize(object[] args)
	{
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.ALL_CARDS);
		var activeDeck = SaveManager.ReadFromJSON<CardDTO>(Settings.ACTIVE_CARDS);
		var inventoryCards = Utilities.GetUniqueItems(allCards, activeDeck);

		activeCardsController.InitializeSlots(activeDeck, CardSlot.Active);
		inventoryController.InitializeSlots(inventoryCards, CardSlot.Inventory);
	}

	public InventoryController GetInventory()
	{
		return inventoryController;
	}

	public ActiveCardsController GetActiveCardsController()
	{
		return activeCardsController;
	}

	private void OnDisable()
	{
		var activeDeck = Cards.Where(x => x.CardSlot == CardSlot.Active);

		// Allows Player to not use any upgrades and removes all cards from Active deck.
		if (SaveManager.GetNumSavedItems<CardDTO>(Settings.ACTIVE_CARDS) == 0)
		{
			foreach (var activeCard in activeDeck)
			{
				SaveManager.DeleteFromJSON<CardDTO>(activeCard.Details.ID, Settings.ACTIVE_CARDS);
			}
		}
		else
		{
			SaveManager.SaveToJSON(activeDeck, Settings.ACTIVE_CARDS);
		}
	}
}
