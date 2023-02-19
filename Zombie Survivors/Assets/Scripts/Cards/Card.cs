using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CardView))]
public class Card : Slot<CardDTO>
{
	public CardView CardView;

	[HideInInspector] public bool IsReadyToUpgrade = false;
	[HideInInspector] public int CardIndex;
	[HideInInspector] public CardSlot CardSlot;

	public override void Initialize(CardDTO slotDetails, int slotIndex, CardSlot cardSlot)
	{
		CardIndex = slotIndex;
		CardView.CardReference = this;
		CardSlot = cardSlot;
		IsEmpty = false;
		Details = slotDetails;

		slotDetails.CardSlot = CardSlot;

		if (slotDetails.Ammount >= Details.CardsRequiredToNextLevel)
		{
			IsReadyToUpgrade = true;
		}

		SaveManager.SaveToJSON(slotDetails, Settings.CARDS);
		EquipmentTab.Add(this);

		CardView.InitializeCardView();
	}

	public override void SetEmpty(int index)
	{
		EquipmentTab.Cards.Remove(this);
		CardView.InitializeEmptyView();
		SlotID = index;
		IsEmpty = true;
		CardSlot = CardSlot.None;
		CardView.CardReference = null;
		Details = null;
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

	public void UseInInventory()
	{
		var inventory = CanvasManager.GetTab<EquipmentTab>().GetInventory();
		if (inventory != null)
		{
			inventory.InitializeSlot(Details, CardSlot.Inventory);
		}
		SetEmpty(SlotID);
	}

	public void UseInActiveDeck()
	{
		// TODO: Add ability to replace
		if (ActiveCardsController.MAX_SLOT_COUNT == EquipmentTab.Cards.Where(x => x.CardSlot == CardSlot.Active).Count())
		{
			print("All active slots are occupied");
			return;
		}
			

		var activeCardsController = CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController();
		if (activeCardsController != null)
		{
			activeCardsController.InitializeSlot(Details, CardSlot.Active);
		}
		SetEmpty(SlotID);
	}


	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}