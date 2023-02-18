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
		characterSelector.InitializeChatacter();

		InitializeSlots();
	}

	private void InitializeSlots()
	{
		Cards = new List<Card>();

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
		var activeDeck = Cards.Where(x => x.CardSlot == CardSlot.Active).Select(x => x.Details).ToList();
		SaveManager.SaveToJSON(activeDeck, Settings.ACTIVE_CARDS);
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			var activeDeck = Cards.Where(x => x.CardSlot == CardSlot.Active).Select(x => x.Details).ToList();
			SaveManager.SaveToJSON(activeDeck, Settings.ACTIVE_CARDS);
		}
	}
}
