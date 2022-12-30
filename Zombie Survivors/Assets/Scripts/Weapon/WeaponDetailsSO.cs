using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : MonoBehaviour
{
	#region Header WEAPON BASE DETAILS
	[Space(10)]
	[Header("WEAPON BASE DETAILS")]
	#endregion Header WEAPON BASE DETAILS
	#region Tooltip
	[Tooltip("Weapon name")]
	#endregion Tooltip
	public string weaponName;

	#region Header WEAPON CONFIGURATION
	[Space(10)]
	[Header("WEAPON CONFIGURATION")]
	#endregion Header WEAPON CONFIGURATION
	#region Tooltip
	[Tooltip("Weapon Shoot Position - the offset position for the end of the weapon from the sprite pivot pont")]
	#endregion Tooltip
	public Vector3 weaponShootPosition;
	#region Tooltip
	[Tooltip("Weapon current ammo")]
	#endregion Tooltip
	public AmmoDetailsSO weaponCurrentAmmo;

	#region Header WEAPON OPERATING VALUES
	[Space(10)]
	[Header("WEAPON OPERATING VALUES")]
	#endregion Header WEAPON OPERATING VALUES
	#region Tooltip
	[Tooltip("Select if the weapon has infinite ammo")]
	#endregion Tooltip
	public bool hasInfiniteAmmo = false;
	#region Tooltip
	[Tooltip("Select if the weapon has infinite clip capacity")]
	#endregion Tooltip
	public bool hasInfiniteClipCapacity = false;
	#region Tooltip
	[Tooltip("The weapon capacity - shots before a reload")]
	#endregion Tooltip
	public int weaponClipAmmoCapacity = 6;
	#region Tooltip
	[Tooltip("Weapon ammo capacity - the maximum number of rounds at that can be held for this weapon")]
	#endregion Tooltip
	public int weaponAmmoCapacity = 100;
	#region Tooltip
	[Tooltip("Weapon Fire Rate - 0.2 means 5 shots a second")]
	#endregion Tooltip
	public float weaponFireRate = 0.2f;
	#region Tooltip
	[Tooltip("This is the weapon reload time in seconds")]
	#endregion Tooltip
	public float weaponReloadTime = 0f;

}
