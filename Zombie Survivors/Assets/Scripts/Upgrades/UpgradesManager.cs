using System;

public static class UpgradesManager
{
	public static event Action<WeaponUpgradeEventArgs> OnWeaponUpgrade;
	public static event Action<AmmoUpgradeEventArgs> OnAmmoUpgrade;
	public static event Action<PlayerStatUpgradeEventArgs> OnPlayerStatUpgrade;

	public static void UpgradeWeapon(WeaponStats weaponStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnWeaponUpgrade?.Invoke(new WeaponUpgradeEventArgs()
		{
			boolValue = cardBoolValue,
			intValue = cardIntValue,
			floatValue = cardFloatValue,
			weaponStats = weaponStat,
			upgradeAction = upgradeAction
		});
	}

	public static void UpgradeAmmo(AmmoStats ammoStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnAmmoUpgrade?.Invoke(new AmmoUpgradeEventArgs()
		{
			boolValue = cardBoolValue,
			intValue = cardIntValue,
			floatValue = cardFloatValue,
			ammoStats = ammoStat,
			upgradeAction = upgradeAction
		});
	}

	public static void UpgradePlayerStat(PlayerStats playerStat, UpgradeAction upgradeAction, float cardFloatValue = 0, int cardIntValue = 0, bool cardBoolValue = false)
	{
		OnPlayerStatUpgrade?.Invoke(new PlayerStatUpgradeEventArgs()
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
