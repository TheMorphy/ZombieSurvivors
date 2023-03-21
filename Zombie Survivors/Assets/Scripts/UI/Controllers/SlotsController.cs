using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotsController<T> : MonoBehaviour 
{
	[SerializeField] private Slot<T>[] slots;

	public virtual void InitializeSlot(T itemDetails, Slot cardSlots)
	{
		int slotIndex = GetEmptySlotIndex();

		if (slotIndex == -1)
		{
			print("No empty slots!");
		}
		else
		{
			slots[slotIndex].Initialize(itemDetails, slotIndex, cardSlots);
		}
	}

	public virtual void InitializeSlots(List<T> itemDetails, Slot cardSlot)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].SetEmpty(i);
		}

		foreach (var item in itemDetails)
		{
			int slotIndex = GetEmptySlotIndex();

			if (slotIndex == -1)
			{
				print("No empty slots!");
			}
			else
			{
				slots[slotIndex].Initialize(item, slotIndex, cardSlot);
			}
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
