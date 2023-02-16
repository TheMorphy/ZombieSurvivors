using System.Collections.Generic;
using UnityEngine;

public class ActiveCardsController : MonoBehaviour
{
	[SerializeField] private Card[] CardsSlots;

	[HideInInspector] public static List<CardSO> ActiveDeck = new List<CardSO>();

	public void InitializeActiveDeck(List<CardSO> deck)
	{
		if(ActiveDeck.Count > 0)
		{
			for (int i = 0; i < CardsSlots.Length; i++)
			{
				if (CardsSlots[i].IsEmpty)
				{
					CardsSlots[i].InitializeCard(deck[i]);
					ActiveDeck.Add(deck[i]);
					deck.Remove(deck[i]);
				}
			}
		}

		if(deck.Count > ActiveDeck.Count)
		{
			var inventory = CanvasManager.GetTab<EquipmentTab>().GetInventory();
			for (int i = 0; i < deck.Count; i++)
			{
				inventory.AddToInventory(deck[i]);	
			}
		}
	}

	public List<Card> GetOccupiedSlots()
	{
		List<Card> slots = new List<Card>();

		foreach (var slot in CardsSlots)
		{
			if (slot.IsEmpty == false)
			{
				slots.Add(slot);
			}
		}

		return slots;
	}
}
