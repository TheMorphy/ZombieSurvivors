using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CardView))]
public class Card : Slot<CardDTO>
{
	public CardView CardView;

	public bool IsReadyToUpgrade = false;
	public int CardIndex;
	public CardSlot CardSlot;

	public override void Initialize(CardDTO slotDetails, int slotIndex, CardSlot cardSlot)
	{
		CardIndex = slotIndex;
		CardView.CardReference = this;
		CardSlot = cardSlot;
		IsEmpty = false;
		Details = slotDetails;
		CardView.InitializeCardView();

		if (slotDetails.Ammount >= Details.CardsRequiredToNextLevel)
		{
			IsReadyToUpgrade = true;
		}

		EquipmentTab.Cards.Add(this);
	}

	public override void SetEmpty()
	{
		IsEmpty = true;
		Details = default;
		CardView.CardReference = this;
		CardView.InitializeEmptyView();

		EquipmentTab.Cards.Add(this);
	}

	public void Upgrade()
	{
		Details.LevelUp();

		if (Details.CanLevelUpCard() == false)
		{
			IsReadyToUpgrade = false;
		}
		CardView.RefreshView();
	}

	public void RemoveFromActiveDeck()
	{
		CanvasManager.GetTab<EquipmentTab>().GetInventory().InitializeSlot(Details, CardSlot.Inventory);
		SetEmpty();
	}

	public void UseInActiveDeck()
	{
		CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().InitializeSlot(Details, CardSlot.Active);
		SetEmpty();
	}

	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}