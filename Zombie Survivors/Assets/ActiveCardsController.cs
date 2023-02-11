using System.Collections.Generic;
using UnityEngine;

public class ActiveCardsController : MonoBehaviour
{
	[SerializeField] private Card[] CardsSlots;

	public List<CardSO> cardsToAdd = new List<CardSO>();

	private void Start()
	{
		for (int i = 0; i < GameResources.Instance.CommonCards.Count; i++)
		{
			if (PlayerPrefs.HasKey(GameResources.Instance.CommonCards[i].CardCode))
			{
				cardsToAdd.Add(GameResources.Instance.CommonCards[i]);
				InitializeAirdropSlotIfEmpty(GameResources.Instance.CommonCards[i]);
			}
		}

		for (int i = 0; i < GameResources.Instance.RareCards.Count; i++)
		{
			if (PlayerPrefs.HasKey(GameResources.Instance.RareCards[i].CardCode))
			{
				cardsToAdd.Add(GameResources.Instance.RareCards[i]);
				InitializeAirdropSlotIfEmpty(GameResources.Instance.RareCards[i]);
			}
		}

		for (int i = 0; i < GameResources.Instance.EpicCards.Count; i++)
		{
			if (PlayerPrefs.HasKey(GameResources.Instance.EpicCards[i].CardCode))
			{
				cardsToAdd.Add(GameResources.Instance.EpicCards[i]);
				InitializeAirdropSlotIfEmpty(GameResources.Instance.EpicCards[i]);
			}
		}
	}

	public void InitializeAirdropSlotIfEmpty(CardSO cardDetails)
	{
		int slotIndex = GetEmptySlotIndex();
		if (slotIndex == -1)
		{
			print("Slots are Full!");
			//TODO: Add to Inventory instead
		}
		else
		{
			CardsSlots[slotIndex].InitializeCard(cardDetails);
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
