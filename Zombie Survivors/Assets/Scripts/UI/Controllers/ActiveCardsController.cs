using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

public class ActiveCardsController : SlotsController<CardDTO>
{
	public const int MAX_SLOT_COUNT = 5;
	public List<Card> ActiveCards = new List<Card>();

	public override void InitializeSlots(List<CardDTO> itemDetails, Slot cardSlot)
	{
		// Initialize all cards to the first empty slot
		base.InitializeSlots(itemDetails, cardSlot);

		var initializedCards = ActiveCards.Where(x => x.Details != null).ToList()
			;
		// Loop through the groups and rearrange the cards
		foreach (var activeCard in initializedCards)
		{
			var requiredSlot = ActiveCards.FirstOrDefault(x => x.CardType == activeCard.Details.CardType);

			// Move all cards in the group to the required slot
			foreach (var card in ActiveCards)
			{
				if (card.CardType == requiredSlot.Details.CardType)
				{
					var tempDetails = requiredSlot.Details;
					requiredSlot.Initialize(card.Details, requiredSlot.SlotIndex, Slot.Active);
					card.Initialize(tempDetails, card.SlotIndex, Slot.Active);
				}
			}
		}
	}


	public void AddCard(Card card)
	{
		if(!ActiveCards.Any(x => x.Details?.Code == card.Details.Code))
			ActiveCards.Add(card);
	}
}
