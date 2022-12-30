using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
	#region Header PLAYER BASE DETAILS
	[Space(10)]
	[Header("PLAYER BASE DETAILS")]
	#endregion
	#region Tooltip
	[Tooltip("Player character name.")]
	#endregion
	public string playerCharacterName;

	#region Tooltip
	[Tooltip("Prefab gameobject for the player")]
	#endregion
	public GameObject playerPrefab;

	#region Header HEALTH
	[Space(10)]
	[Header("HEALTH")]
	#endregion
	#region Tooltip
	[Tooltip("Player starting health amount")]
	#endregion
	public int playerHealthAmount;

	#region Header WEAPON
	[Space(10)]
	[Header("WEAPON")]
	#endregion
	#region Tooltip
	[Tooltip("Player  initial starting weapon")]
	#endregion
	public WeaponDetailsSO startingWeapon;

}
