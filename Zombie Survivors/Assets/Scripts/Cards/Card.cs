using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CardView))]
[RequireComponent(typeof(CardAnimation))]
public class Card : Slot<CardDTO>
{
	public CardView CardView;
	
	public bool IsReadyToUpgrade = false;
	public int CardIndex;
	public CardSlot CardSlot;

	[HideInInspector] public CardAnimation CardAnimation;

	public override void Initialize(CardDTO slotDetails, int slotIndex, CardSlot cardSlot)
	{
		CardAnimation = GetComponent<CardAnimation>();	
		CardIndex = slotIndex;
		CardView.CardReference = this;
		CardSlot = cardSlot;
		IsEmpty = false;
		Details = slotDetails;
		slotDetails.CardSlot = CardSlot;
		CardView.InitializeCardView();

		SaveManager.SaveToJSON(slotDetails, Settings.CARDS);
		EquipmentTab.Add(this);
	}

	public override void SetEmpty(int index)
	{
		EquipmentTab.Cards.Remove(this);
		CardView.InitializeEmptyView();
		SlotID = index;
		IsEmpty = true;
		CardSlot = CardSlot.None;
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
			SetEmpty(SlotID);
		}
	}

	public void UseInActiveDeck()
	{
		var activeCardsController = CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController();
		// TODO: Add ability to replace
		if (activeCardsController.GetSlots().Where(x => !x.IsEmpty).Count().Equals(ActiveCardsController.MAX_SLOT_COUNT))
		{
			print("All active slots are occupied");
			return;
		}

		if (activeCardsController != null)
		{
			activeCardsController.InitializeSlot(Details, CardSlot.Active);
			SetEmpty(SlotID);
		}
	}


	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}