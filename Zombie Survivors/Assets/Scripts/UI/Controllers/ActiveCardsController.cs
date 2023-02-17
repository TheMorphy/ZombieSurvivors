using System.Collections.Generic;
using UnityEngine;

public class ActiveCardsController : SlotsController<CardDTO>
{
	public const int MAX_SLOT_COUNT = 5;

	[HideInInspector] public static List<Card> ActiveDeck = new List<Card>();
}
