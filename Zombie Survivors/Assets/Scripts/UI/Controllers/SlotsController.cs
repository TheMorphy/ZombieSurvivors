using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotsController<T> : MonoBehaviour 
{
	[SerializeField] private Slot<T>[] slots;

	public void InitializeSlot(T slotDetails)
	{
		int slotIndex = GetEmptySlotIndex();

		if (slotIndex == -1)
		{
			print("No empty slots!");
		}
		else
		{
			slots[slotIndex].Initialize(slotDetails, slotIndex);
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

	public List<Slot<T>> GetSlots()
	{
		return slots.ToList();
	}

	public Slot<T> GetSlot(int slotIndex)
	{
		return slots[slotIndex];
	}
}
