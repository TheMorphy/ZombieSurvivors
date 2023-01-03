using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Weapons/Ammo Detail")]
public class AmmoDetailsSO : ScriptableObject
{
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
	public bool isAmmoTrail = false;
	#region Tooltip
	[Tooltip("Ammo trail lifetime in seconds.")]
	#endregion
	public float ammoTrailTime = 3f;
	#region Tooltip
	[Tooltip("Ammo trail material.")]
	#endregion
	public Material ammoTrailMaterial;
	#region Tooltip
	[Tooltip("The starting width for the ammo trail.")]
	#endregion
	[Range(0f, 1f)] public float ammoTrailStartWidth;
	#region Tooltip
	[Tooltip("The ending width for the ammo trail")]
	#endregion
	[Range(0f, 1f)] public float ammoTrailEndWidth;
}
