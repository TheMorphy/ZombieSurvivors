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
	public bool InfiniteAmmo = true;
	
	[Tooltip("Can shoot endlessly")]
	public bool InfiniteMagazineCapacity = true;

	[Tooltip("Fires several bullets in quick sequence. If set TRUE, specify the burst bullet ammount")]
	public bool BurstFire = false;

	[Tooltip("Fires several bullets in single shot. If set TRUE, specify the spread shot bullet ammount")]
	public bool SpreadShot = false;

	[Tooltip("Reload time in seconds")]
	public float ReloadTime = 0.5f;

	[Tooltip("Total ammo capacity")]
	public int AmmoCapacity = 50;

	[Tooltip("Ammo capacity in the magazine")]
	public int MagazineSize = 50;

	[Min(0.01f)]
	[Tooltip("The fire rate of the weapon, in shots per second")]
	public float FireRate = 5.0f;

	[Tooltip("The time between bursts")]
	public float BurstInterval = 0.5f;

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

