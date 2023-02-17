using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;

	[HideInInspector] public static List<Card> Cards = new List<Card>();

	public override void Initialize(object[] args)
	{
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.ALL_CARDS);
		var activeDeck = SaveManager.ReadFromJSON<CardDTO>(Settings.ACTIVE_CARDS);
		var inventoryCards = Utilities.GetUniqueItems(allCards, activeDeck);

		activeCardsController.InitializeSlots(activeDeck);
		inventoryController.InitializeSlots(inventoryCards);
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
		// Allows Player to not use any upgrades and removes all cards from Active deck.
		if (SaveManager.GetNumSavedItems<CardDTO>(Settings.ACTIVE_CARDS) == 0)
		{
			foreach (var activeCard in ActiveCardsController.ActiveDeck)
			{
				SaveManager.DeleteFromJSON<CardDTO>(activeCard.Details.ID, Settings.ACTIVE_CARDS);
			}
		}
		else
		{
			SaveManager.SaveToJSON(ActiveCardsController.ActiveDeck, Settings.ACTIVE_CARDS);
		}
	}

	public static void Add(Card card)
	{
		if (!Cards.Contains(card))
		{
			Cards.Add(card);
		}
	}
}
