using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotsController : MonoBehaviour
{
	public static SlotsController Instance;

	[SerializeField]
	private Slot[] Slots;
	public bool IsUnlocking;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		IsUnlocking = false;
	}

	public void InitializeAirdropSlotIfEmpty(AirdropDetails airdropDetails, int index)
	{
		int slot = GetEmptySlot();
		if (slot == -1)
		{
			//UIHandler.Instance.ToggleSlotsFullPopup(true);
			print("Slots are Full!");
		}
		else
		{
			Slots[slot].SetAirdropSlot(airdropDetails, index);
		}
	}

	private int GetEmptySlot()
	{
		for (int i = 0; i < Slots.Length; i++)
		{
			if (Slots[i].IsEmpty)
			{
				return i;
			}
		}
		return -1;
	}

	public List<Slot> GetOccupiedSlots()
	{
		List<Slot> slots = new List<Slot>();

		foreach (var slot in Slots)
		{
			if(slot.IsEmpty == false)
			{
				slots.Add(slot);
			}
		}

		return slots;
	}

	public void Hide()
	{
		transform.parent.gameObject.SetActive(false);
	}
}
