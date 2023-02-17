using System.Collections.Generic;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;

	[SerializeField] private List<CardDTO> allCards;
	[SerializeField] private List<CardDTO> activeDeck;

	public override void Initialize(object[] args)
	{
		InitializeSlots();
	}

	private void InitializeSlots()
	{
		var allCards = SaveManager.ReadFromJSON<CardDTO>(Settings.ALL_CARDS);
		var savedActiveDeck = SaveManager.ReadFromJSON<CardDTO>(Settings.ACTIVE_CARDS);
		var inventoryCards = Utilities.GetUniqueItems(allCards, activeDeck);

		if (inventoryCards.Count > 0)
		{
			foreach (var card in inventoryCards)
			{
				inventoryController.InitializeSlot(card);
			}
		}

		if (savedActiveDeck.Count > 0)
		{
			foreach (var card in savedActiveDeck)
			{
				activeCardsController.InitializeSlot(card);
			}
		}

		if(savedActiveDeck.Count == 0)
		{
			for (int i = 0; i < activeCardsController.GetSlots().Count; i++)
			{
				activeCardsController.GetSlots()[i].SetEmpty();
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
}
