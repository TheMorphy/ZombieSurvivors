using System.Collections.Generic;
using UnityEngine;

public class ActiveCardsController : MonoBehaviour
{
	[SerializeField] private Card[] CardsSlots;

	[HideInInspector] public static List<CardSO> ActiveDeck = new List<CardSO>();

	public void InitializeActiveDeck(List<CardSO> activeDeck)
	{
		if(ActiveDeck.Count > 0)
		{
			for (int i = 0; i < CardsSlots.Length; i++)
			{
				if (CardsSlots[i].IsEmpty)
				{
					CardsSlots[i].InitializeCard(activeDeck[i]);
					ActiveDeck.Add(activeDeck[i]);
				}
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
