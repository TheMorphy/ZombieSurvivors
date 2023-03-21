using UnityEngine;

public abstract class Slot<T> : MonoBehaviour
{
	public T? Details;
	public bool IsEmpty = true;
	public int SlotIndex;

	public abstract void Initialize(T slotDetails, int slotIndex, Slot slotType);
	public abstract void SetEmpty(int slotIndex);
	public virtual void Open() { }
	public virtual void Select() { }
}

public enum Slot
{
	None,
	Active,
	Inventory
}