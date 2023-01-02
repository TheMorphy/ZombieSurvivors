using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Weapons/Ammo Detail")]
public class AmmoDetailsSO : ScriptableObject
{
	[Tooltip("The damage that a single bullet will deal to the hit object")]
	public int ammoDamage = 10;

	[Tooltip("Specify the burst bullet ammount")]
	public int? busrtFireBulletCount = 5;

	#region Tooltip
	[Tooltip("The range of the ammo in unity units")]
	#endregion
	public float ammoRange = 20f;

	public float ammoSpeed = 100;

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
