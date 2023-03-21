using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CardView))]
public class Card : Slot<CardDTO>
{
	[SerializeField] private CardType cardType;
	public CardType CardType { get { return cardType; } }
	public CardView CardView;

	[HideInInspector] public bool IsReadyToUpgrade = false;
	[HideInInspector] public int CardIndex;
	[HideInInspector] public Sprite CardSprite;
	[HideInInspector] public Slot CardSlot;

	public override void Initialize(CardDTO slotDetails, int slotIndex, Slot cardSlot)
	{
		CardView.CardReference = this;

		CardIndex = slotIndex;
		CardSlot = cardSlot;
		IsEmpty = false;
		Details = slotDetails;
		slotDetails.CardSlot = CardSlot;
		CardSprite = GameResources.Instance.GetCardSprite(slotDetails.CardType, slotDetails.CardRarity); 
		CardView.InitializeCardView();

		SaveManager.SaveToJSON(slotDetails, Settings.CARDS);
		EquipmentTab.AddInitializedCard(this);
	}

	public override void SetEmpty(int index)
	{
		CardView.InitializeEmptyView();
		SlotIndex = index;
		IsEmpty = true;
		CardSlot = Slot.None;
		Details = null;

		// Only Active deck card slots should have the option to specify the card type it accpets.
		// Since cards in inventory don't require specific type of slot, it is set to Any.
		if(CardType != CardType.Any && !CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().ActiveCards.Any(x => x.SlotIndex == index))
			CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().ActiveCards.Add(this);
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
			inventory.InitializeSlot(Details, Slot.Inventory);
			SetEmpty(SlotIndex);
		}
	}

	public void UseInActiveDeck()
	{
		// Find the card slot in active deck, that accepts the current card type
		var availableCardToInitialize = CanvasManager.GetTab<EquipmentTab>().GetActiveCardsController().ActiveCards.First(activeCard => activeCard.cardType == Details.CardType);
		var tmpDetails = Details;

		// If the card slot is empty, then just initialize there
		if (availableCardToInitialize.IsEmpty)
		{
			availableCardToInitialize.Initialize(tmpDetails, availableCardToInitialize.SlotIndex, Slot.Active);
			SetEmpty(SlotIndex);
		}
		// Else, switch places with the card, that is already initialized
		else
		{
			Initialize(availableCardToInitialize.Details, SlotIndex, Slot.Inventory);
			availableCardToInitialize.Initialize(tmpDetails, availableCardToInitialize.SlotIndex, Slot.Active);
		}
	}


	public void DisplayCardInfo()
	{
		// TODO: Card info window
	}
}