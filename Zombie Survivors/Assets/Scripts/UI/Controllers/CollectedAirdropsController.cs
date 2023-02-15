using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectedAirdropsController : MonoBehaviour
{
	[SerializeField] private Slot[] slots;

	public void InitializeAirdropSlotIfEmpty(AirdropDTO airdropDetails)
	{
		int slotIndex = GetEmptySlotIndex();

		if(slotIndex == -1)
		{
			print("No empty slots!");
		}
		else
		{
			slots[slotIndex].InitializeAirdropSlot(airdropDetails, slotIndex);
		}
	}

	private int GetEmptySlotIndex()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i].IsEmpty)
			{
				return i;
			}
		}
		return -1;
	}

	public List<Slot> GetSlots()
	{
		return slots.ToList();
	}

	public Slot GetSlot(int slotIndex)
	{
		return slots[slotIndex];
	}
}
