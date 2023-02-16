using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;

	[SerializeField] private List<CardSO> AllGear;
	[SerializeField] private List<CardSO> ActiveDeck;

	public override void Initialize()
	{
		GetAllGear();
		InitializeInventory();
		InitializeActiveDeck();
	}

	public override void InitializeWithArgs(object[] args)
	{
		
	}

	private void InitializeInventory()
	{
		if (AllGear.Count == 0)
		{
			inventoryController.InitializeEmptyInventory();
		}
		else
		{
			
		}
	}

	private void InitializeActiveDeck()
	{
		ActiveDeck = SaveManager.ReadFromJSON<CardSO>(Settings.ACTIVE_UPGRADES);
		activeCardsController.InitializeActiveDeck(ActiveDeck);
	}

	private void GetAllGear()
	{
		AllGear = SaveManager.ReadFromJSON<CardSO>(Settings.ALL_CARDS);
	}

	public InventoryController GetInventory()
	{
		return inventoryController;
	}

	private void OnDisable()
	{
		SaveManager.InsertToJSON(ActiveCardsController.ActiveDeck, Settings.ACTIVE_UPGRADES);
	}
}
