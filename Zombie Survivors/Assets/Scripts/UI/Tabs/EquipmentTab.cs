using System.Collections.Generic;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;

	[SerializeField] private List<CardDTO> AllCards;
	[SerializeField] private List<CardDTO> ActiveDeck;

	public override void Initialize()
	{
		GetAllCards();
		InitializeInventory();
		InitializeActiveDeck();
	}

	public override void InitializeWithArgs(object[] args)
	{
		
	}

	private void InitializeInventory()
	{
		if (AllCards.Count == 0)
		{
			inventoryController.InitializeEmptyInventory();
		}
	}

	private void InitializeActiveDeck()
	{
		ActiveDeck = SaveManager.ReadFromJSON<CardDTO>(Settings.ACTIVE_UPGRADES);

		if(ActiveDeck.Count == 0)
		{
			activeCardsController.InitializeActiveDeck(AllCards);
		}
		else
		{
			activeCardsController.InitializeActiveDeck(ActiveDeck);
		}	
	}

	private void GetAllCards()
	{
		AllCards = SaveManager.ReadFromJSON<CardDTO>(Settings.ALL_CARDS);
	}

	public InventoryController GetInventory()
	{
		return inventoryController;
	}

	private void OnDisable()
	{
		// Allows Player to not use any upgrades and removes all cards from Active deck.
		if (SaveManager.GetNumSavedItems<CardDTO>(Settings.ACTIVE_UPGRADES) == 0)
		{
			foreach (var activeCard in ActiveCardsController.ActiveDeck)
			{
				SaveManager.DeleteFromJSON<CardDTO>(activeCard.CardDetails.ID, Settings.ACTIVE_UPGRADES);
			}
		}
		else
		{
			SaveManager.SaveToJSON(ActiveCardsController.ActiveDeck, Settings.ACTIVE_UPGRADES);
		}
	}
}
