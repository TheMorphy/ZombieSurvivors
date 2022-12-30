using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
	#region Header BASIC AMMO DETAILS
	[Space(10)]
	[Header("BASIC AMMO DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("Name for the ammo")]
	#endregion
	public string ammoName;

	#region Header AMMO SPRITE, PREFAB & MATERIALS
	[Space(10)]
	[Header("AMMO SPRITE, PREFAB & MATERIALS")]
	#endregion
	public GameObject ammoPrefab;

	#region Tooltip
	[Tooltip("The damage each ammo deals")]
	#endregion
	public int ammoDamage = 5;
	#region Tooltip
	[Tooltip("The minimum speed of the ammo - the speed will be a random value between the min and max")]
	#endregion
	public float ammoSpeed = 20f;
	#region Tooltip
	[Tooltip("The range of the ammo (or ammo pattern) in unity units")]
	#endregion
	public float ammoRange = 20f;

	#region Header AMMO SPREAD DETAILS
	[Space(10)]
	[Header("AMMO SPREAD DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("This is the  minimum spread angle of the ammo.  A higher spread means less accuracy. A random spread is calculated between the min and max values.")]
	#endregion
	public float ammoSpreadMin = 0f;
	#region Tooltip
	[Tooltip(" This is the  maximum spread angle of the ammo.  A higher spread means less accuracy. A random spread is calculated between the min and max values. ")]
	#endregion
	public float ammoSpreadMax = 0f;

	#region Header AMMO SPAWN DETAILS
	[Space(10)]
	[Header("AMMO SPAWN DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("This is the minimum number of ammo that are spawned per shot. A random number of ammo are spawned between the minimum and maximum values. ")]
	#endregion
	public int ammoSpawnAmountMin = 1;
	#region Tooltip
	[Tooltip("This is the maximum number of ammo that are spawned per shot. A random number of ammo are spawned between the minimum and maximum values. ")]
	#endregion
	public int ammoSpawnAmountMax = 1;
	#region Tooltip
	[Tooltip("Minimum spawn interval time. The time interval in seconds between spawned ammo is a random value between the minimum and maximum values specified.")]
	#endregion
	public float ammoSpawnIntervalMin = 0f;
	#region Tooltip
	[Tooltip("Maximum spawn interval time. The time interval in seconds between spawned ammo is a random value between the minimum and maximum values specified.")]
	#endregion
	public float ammoSpawnIntervalMax = 0f;
}
