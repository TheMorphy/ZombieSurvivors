using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveCardsController : MonoBehaviour
{
	[SerializeField] private Card[] CardsSlots;

	[HideInInspector] public static List<Card> ActiveDeck = new List<Card>();

	public void InitializeActiveDeck(List<CardDTO> deck)
	{
		if (deck.Count > 0)
		{
			foreach (var card in deck)
			{
				int index = GetEmptySlotIndex();

				CardsSlots[index].InitializeCard(card);
				ActiveDeck.Add(CardsSlots[index]);
			}

			CardsSlots.ToList().ForEach((slot) =>
			{
				if (slot.IsEmpty)
				{
					slot.InitializeEmptyCard();
				}
			});
		}
	}

	private int GetEmptySlotIndex()
	{
		for (int i = 0; i < CardsSlots.Length; i++)
		{
			if (CardsSlots[i].IsEmpty)
			{
				return i;
			}
		}
		return -1;
	}
}
