using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum WeaponStats
{
	[Description("Infinite Ammo")]
	hasInfiniteAmmo,
	[Description("Infinite Clip Capacity")]
	hasInfiniteClipCapacity,
	[Description("Weapon Burst Fire")]
	burstFire,
	[Description("Weapon Spread Shot")]
	spreadShot,
	[Description("Weapon Reload Time")]
	weaponReloadTime,
	[Description("Weapon Total Ammo Capacity")]
	weaponAmmoCapacity,
	[Description("Weapn Clip Capacity")]
	weaponClipAmmoCapacity,
	[Description("Weapn Fire Rate")]
	fireRate,
	[Description("Weapn Burst Interval")]
	burstInterval
}

[CreateAssetMenu(fileName = "Gun_", menuName = "Scriptable Objects/Weapons/Gun")]
public class WeaponDetailsSO : ScriptableObject
{
	[Space]
	[SerializeField]
	private WeaponType type;
	public WeaponType Type { get { return type; } }

	[Space(5)]
	[SerializeField]
	private Sprite weaponPicture;
	public Sprite WeaponPicture { get { return weaponPicture; } }

	public AmmoDetailsSO AmmoDetails;

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

	#region Header UPGRADES
	[Space(5)]
	[Header("AVAILABLE UPGRADES")]
	#endregion Header UPGRADES
	[Space(2)]
	[Header("Weapon Upgrades")]
	[SerializeField]
	private List<WeaponUpgradeDetails> weaponUpgrades;
	public List<WeaponUpgradeDetails> WeaponUpgrades { get { return weaponUpgrades; } }
}

public enum WeaponType
{
	Pistol,
	Shotgun,
	Rifle,
	SMG
}

