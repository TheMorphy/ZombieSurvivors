using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum PlayerStats
{
	[Description("Player Health")]
	Health,
	[Description("Player Move Speed")]
	MoveSpeed
}

[CreateAssetMenu(fileName = "PlayerDetails", menuName = "Scriptable Objects/Characters/Player")]
public class PlayerDetailsSO : ScriptableObject
{
	[SerializeField]
	private PlayerClass playerClass;
	public PlayerClass PlayerClass { get { return playerClass; } }

	[SerializeField]
	private string playerName;
	public string Name { get { return playerName; } }

	[SerializeField]
	private Sprite playerPicture;
	public Sprite PlayerPicture { get { return playerPicture; } }

	[SerializeField]
	private GameObject comradePrefab;
	public GameObject ComradePrefab { get { return comradePrefab; } }

	[SerializeField]
	private WeaponDetailsSO playerWeaponDetails;
	public WeaponDetailsSO PlayerWeaponDetails { get { return playerWeaponDetails; } }

	public int Health = 100;

	public float MoveSpeed = 5f;

	#region Header UPGRADES
	[Space(5)]
	[Header("AVAILABLE UPGRADES")]
	#endregion Header UPGRADES
	[Space(2)]
	[Header("Player Upgrades")]
	[SerializeField]
	private List<PlayerStatsUpgradeDetails> playerStatsUpgrades;
	public List<PlayerStatsUpgradeDetails> PlayerStatsUpgrades { get { return playerStatsUpgrades; } }
}

public enum PlayerClass
{
	Soldier,
	Juggernaut,
	Fast_Soldier
}
