using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentTab : Tab
{
	[SerializeField] private ActiveCardsController activeCardsController;
	[SerializeField] private InventoryController inventoryController;
	[SerializeField] private CharacterSelector characterSelector;

	[HideInInspector] public static List<Card> Cards = new List<Card>();

	public override void Initialize(object[] args)
	{
		// Checks if any new cards were opened from opening airdrop. If so, add those to inventory
		// the rest ignore, since it would cause duplication
		if(args != null)
		{
			var cardsObtained = (List<CardDTO>)args[0];

			var uniqueCards = Utilities.GetUniqueItems(cardsObtained, Cards.Select(x => x.Details).ToList());
			inventoryController.InitializeSlots(uniqueCards, CardSlot.Inventory);
			return;
		}

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

	public InventoryController GetInventory()
	{
		return inventoryController;
	}

	public ActiveCardsController GetActiveCardsController()
	{
		return activeCardsController;
	}
}
