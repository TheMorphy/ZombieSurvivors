using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum AmmoStats
{
	[Description("Ammo Damage")]
	AmmoDamage,
	[Description("Ammo Per Shot")]
	AmmoPerShot,
	[Description("Ammo Range")]
	AmmoRange,
	[Description("Ammo Speed")]
	AmmoSpeed,
	[Description("Ammo Spread Angle")]
	AmmoShootAngle
}


[CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Weapons/Ammo Detail")]
public class AmmoDetailsSO : BaseScriptableObject
{
	[SerializeField]
	private Sprite ammoPicture;
	public Sprite AmmoPicture { get { return ammoPicture; } }

	[Tooltip("The damage that a single bullet will deal to the hit object")]
	public int ammoDamage = 10;

	[Tooltip("Ammount of ammo will be fired in one shot")]
	public int ammoPerShot = 1;

	#region Header AMMO SPREAD DETAILS
	[Space(10)]
	[Header("AMMO SPREAD DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("The range of the ammo in unity units")]
	#endregion
	public float ammoRange = 20f;
	#region Tooltip
	[Tooltip("How quickly the bullets fly")]
	#endregion
	public float ammoSpeed = 100;

	[Tooltip("Ammo direction offset on X and Z axis (in degrees). Don't go too high")]
	public float ammoShootAngle = 0;

	#region Header AMMO TRAIL DETAILS
	[Space(10)]
	[Header("AMMO TRAIL DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("Selected if an ammo trail is required, otherwise deselect.  If selected then the rest of the ammo trail values should be populated.")]
	#endregion
	[SerializeField]
	private bool isAmmoTrail = false;
	public bool IsAmmoTrail { get { return isAmmoTrail; } }
	#region Tooltip
	[Tooltip("Ammo trail lifetime in seconds.")]
	#endregion
	[SerializeField]
	private float ammoTrailTime = 3f;
	public float AmmoTrailTime { get { return ammoTrailTime; } }
	#region Tooltip
	[Tooltip("Ammo trail material.")]
	#endregion
	[SerializeField]
	private Material ammoTrailMaterial;
	public Material AmmoTrailMaterial { get { return ammoTrailMaterial; } }
	#region Tooltip
	[Tooltip("The starting width for the ammo trail.")]
	#endregion
	[SerializeField]
	[Range(0f, 1f)] 
	private float ammoTrailStartWidth;
	public float AmmoTrailStartWidth { get { return ammoTrailStartWidth; } }
	#region Tooltip
	[Tooltip("The ending width for the ammo trail")]
	#endregion
	[SerializeField]
	[Range(0f, 1f)] 
	private float ammoTrailEndWidth;
	public float AmmoTrailEndWidth { get { return ammoTrailEndWidth; } }

	#region Header UPGRADES
	[Space(5)]
	[Header("AVAILABLE UPGRADES")]
	#endregion Header UPGRADES
	[Space(2)]
	[Header("Ammo Upgrades")]
	[SerializeField]
	private List<AmmoUpgradeDetails> ammoUpgrades;
	public List<AmmoUpgradeDetails> AmmoUpgrades { get { return ammoUpgrades; } }
}
