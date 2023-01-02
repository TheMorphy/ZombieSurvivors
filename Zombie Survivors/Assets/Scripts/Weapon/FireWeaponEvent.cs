using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
	public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

	public void CallFireWeaponEvent(bool fire)
	{
		OnFireWeapon?.Invoke(this, new FireWeaponEventArgs() { fire = fire });
	}
}

public class FireWeaponEventArgs : EventArgs
{
	public bool fire;
}