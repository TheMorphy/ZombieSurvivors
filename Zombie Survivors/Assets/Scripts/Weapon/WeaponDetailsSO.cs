using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun_", menuName = "Scriptable Objects/Weapons/Gun")]
public class WeaponDetailsSO : ScriptableObject
{
	[Space]
	public WeaponType Type;

	public Vector3 weaponShootPosition;

	public AmmoDetailsSO ammoDetails;

	[Tooltip("Has unlimited ammo")]
	public bool hasInfiniteAmmo = true;
	
	[Tooltip("Can shoot endlessly")]
	public bool hasInfiniteClipCapacity = true;

	[Tooltip("Fires several bullets in quick sequence. If set TRUE, specify the burst bullet ammount")]
	public bool burstFire = false;

	[Tooltip("Fires several bullets in single shot. If set TRUE, specify the spread shot bullet ammount")]
	public bool spreadShot = false;

	[Tooltip("The fire rate of the weapon, in shots per second")]
	public float weaponReloadTime = 0.5f;

	[Tooltip("The total ammount of ammo that fits in a weapon")]
	public int weaponAmmoCapacity = 50;

	[Tooltip("The ammo count in clip")]
	public int weaponClipAmmoCapacity = 50;

	[Min(0.01f)]
	[Tooltip("The fire rate of the weapon, in shots per second")]
	public float fireRate = 5.0f;

	[Tooltip("The time between bursts")]
	public float burstInterval = 0.5f;
}

public enum WeaponType
{
	Pistol,
	Shotgun,
	Rifle,
	SMG
}