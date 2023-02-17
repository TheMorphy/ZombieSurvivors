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
		CardView.InitializeCardView();

		if (slotDetails.Ammount >= Details.CardsRequiredToNextLevel)
		{
			IsReadyToUpgrade = true;
		}

		EquipmentTab.Add(this);
	}

	public override void SetEmpty()
	{
		IsEmpty = true;
		Details = default;
		CardView.CardReference = this;
		CardView.InitializeEmptyView();
	}

	public void Upgrade()
	{
		Details.LevelUpCard();
		CardView.RefreshView();
	}

	public void RemoveFromActiveDeck()
	{
		ActiveCardsController.ActiveDeck.Remove(this);
		CanvasManager.GetTab<EquipmentTab>().GetInventory().InitializeSlot(Details);
		SetEmpty();
	}

	public void UseInActiveDeck()
	{
		if(!ActiveCardsController.ActiveDeck.Contains(this)) 
		{
			var freeSlot = CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().GetSlots().First(x => x.IsEmpty);
			CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().InitializeSlot(Details);
			SetEmpty();
		}
	}

	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}