using System;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
	#region SINLETON
	public static UpgradesManager Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
	#endregion

	public event Action<UpgradesManager, WeaponUpgradeEventArgs> OnWeaponUpgrade;
	public event Action<UpgradesManager, AmmoUpgradeEventArgs> OnAmmoUpgrade;
	public event Action<UpgradesManager, PlayerStatUpgradeEventArgs> OnPlayerStatUpgrade;

	public void UpgradeWeapon(WeaponStats weaponStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnWeaponUpgrade?.Invoke(this, new WeaponUpgradeEventArgs()
		{
			boolValue = cardBoolValue,
			intValue = cardIntValue,
			floatValue = cardFloatValue,
			weaponStats = weaponStat,
			upgradeAction = upgradeAction
		});
	}

	public void UpgradeAmmo(AmmoStats ammoStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnAmmoUpgrade?.Invoke(this, new AmmoUpgradeEventArgs()
		{
			boolValue = cardBoolValue,
			intValue = cardIntValue,
			floatValue = cardFloatValue,
			ammoStats = ammoStat,
			upgradeAction = upgradeAction
		});
	}

	public void UpgradePlayerStat(PlayerStats playerStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnPlayerStatUpgrade?.Invoke(this, new PlayerStatUpgradeEventArgs()
		{
			boolValue = cardBoolValue,
			intValue = cardIntValue,
			floatValue = cardFloatValue,
			playerStats = playerStat,
			upgradeAction = upgradeAction
		});
	}
}

public class WeaponUpgradeEventArgs : EventArgs
{
	public float floatValue;
	public int intValue;
	public bool boolValue;
	public WeaponStats weaponStats;
	public UpgradeAction upgradeAction;
}

public class AmmoUpgradeEventArgs : EventArgs
{
	public float floatValue;
	public int intValue;
	public bool boolValue;
	public AmmoStats ammoStats;
	public UpgradeAction upgradeAction;
}

public class PlayerStatUpgradeEventArgs : EventArgs
{
	public float floatValue;
	public int intValue;
	public bool boolValue;
	public PlayerStats playerStats;
	public UpgradeAction upgradeAction;
}
