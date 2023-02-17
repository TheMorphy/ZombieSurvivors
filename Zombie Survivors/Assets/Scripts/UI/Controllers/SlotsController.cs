using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotsController<T> : MonoBehaviour 
{
	[SerializeField] private Slot<T>[] slots;

	private void Awake()
	{
		if(slots.Length != transform.childCount)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				slots[i] = transform.GetChild(i).GetComponent<Slot<T>>();
			}
		}
	}

	public virtual void InitializeSlot(T slotDetails, CardSlot cardSlots)
	{
		int slotIndex = GetEmptySlotIndex();

		if (slotIndex == -1)
		{
			print("No empty slots!");
		}
		else
		{
			slots[slotIndex].Initialize(slotDetails, slotIndex, cardSlots);
		}
	}

	public virtual void InitializeSlots(List<T> itemDetails, CardSlot cardSlot)
	{
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

		if (itemDetails.Count < GetSlots().Count)
		{
			GetSlots().ForEach(x => {
				if (x.IsEmpty)
				{
					x.SetEmpty();
				}
			});
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
