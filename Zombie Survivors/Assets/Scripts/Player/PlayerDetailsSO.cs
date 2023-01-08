using System;
using System.Collections;
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
public class PlayerDetailsSO : BaseScriptableObject
{
	[SerializeField]
	private PlayerClass playerClass;
	public PlayerClass PlayerClass { get { return playerClass; } }

	[SerializeField]
	private Sprite playerPicture;
	public Sprite PlayerPicture { get { return playerPicture; } }

	[SerializeField]
	private GameObject playerPrefab;
	public GameObject PlayerPrefab { get { return playerPrefab; } }

	[SerializeField]
	private WeaponDetailsSO startingWeaponDetails;
	public WeaponDetailsSO StartingWeaponDetails { get { return startingWeaponDetails; } }

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

	public void UpgradPlayerBaseStats(PlayerStats statType, float value, UpgradeAction upgradeAction)
	{
		switch(statType)
		{
			case PlayerStats.MoveSpeed:
				if (upgradeAction == UpgradeAction.Add)
					MoveSpeed += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					MoveSpeed *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					MoveSpeed = Utilities.ApplyPercentage(value, MoveSpeed);
				break;

			case PlayerStats.Health:
				if (upgradeAction == UpgradeAction.Add)
					Health += (int)value;
				else if (upgradeAction == UpgradeAction.Multiply)
					Health = (int)(Health * value);
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					Health = (int)Utilities.ApplyPercentage(value, Health);
				break;
		}
	}
}

public enum PlayerClass
{
	Soldier,
	Juggernaut,
	Fast_Soldier
}
