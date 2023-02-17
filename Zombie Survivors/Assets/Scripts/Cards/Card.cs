using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CardView))]
public class Card : Slot<CardDTO>
{
	public CardView CardView;

	public bool IsReadyToUpgrade = false;
	public int CardIndex;

	public override void Initialize(CardDTO slotDetails, int slotIndex)
	{
		CardIndex = slotIndex;
		CardView.CardReference = this;

		IsEmpty = false;
		Details = slotDetails;
		CardView.UpdateCardView();

		if (slotDetails.Ammount >= Details.CardsRequiredToNextLevel)
		{
			IsReadyToUpgrade = true;
		}
	}

	public override void SetEmpty()
	{
		IsEmpty = true;
		CardView.CardReference = this;
		CardView.InitializeEmptyView();
	}

	public void Upgrade()
	{
		Details.UpgradeCard();
		CardView.UpdateCardView();
	}

	public void RemoveFromActiveDeck()
	{
		ActiveCardsController.ActiveDeck.Remove(this);
		CanvasManager.GetTab<EquipmentTab>().GetInventory().InitializeSlot(Details);
	}

	public void UseInActiveDeck()
	{
		var freeSlot = CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().GetSlots().First(x => x.IsEmpty);

		CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().InitializeSlot(Details);
	}

	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}