using UnityEngine;

public abstract class Slot<T> : MonoBehaviour
{
	public T Details;
	public bool IsEmpty = true;
	public int SlotID;

	public abstract void Initialize(T slotDetails, int slotIndex);
	public abstract void SetEmpty();
	public virtual void Open() { }
	public virtual void Select() { }
}