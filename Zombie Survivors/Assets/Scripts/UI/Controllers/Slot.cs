using UnityEngine;

public abstract class Slot<T> : MonoBehaviour
{
	public T Details;
	public bool IsEmpty = true;
	public int SlotID;

	public abstract void Initialize(T slotDetails, int slotIndex, CardSlot cardSlot);
	public abstract void SetEmpty(int slotIndex);
	public virtual void Open() { }
	public virtual void Select() { }
}

public enum CardSlot
{
	None,
	Active,
	Inventory
}