using UnityEngine;

public class Weapon
{
	public WeaponDetailsSO weaponDetails;
	public AmmoDetailsSO ammoDetails;

	public int weaponListPosition;
	public float weaponReloadTimer;
	public int weaponClipRemainingAmmo;
	public int weaponRemainingAmmo;
	public bool isWeaponReloading;

	public void UpgradeWeapon(WeaponStats weaponStat, float value, UpgradeAction upgradeAction)
	{
		switch (weaponStat)
		{
			case WeaponStats.weaponAmmoCapacity:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.weaponAmmoCapacity *= (int)value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.weaponAmmoCapacity += (int)value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.weaponAmmoCapacity = (int)Utilities.ApplyPercentage(value, weaponDetails.weaponAmmoCapacity);
				break;

			case WeaponStats.weaponClipAmmoCapacity:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.weaponClipAmmoCapacity *= (int)value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.weaponClipAmmoCapacity += (int)value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.weaponClipAmmoCapacity = (int)Utilities.ApplyPercentage(value, weaponDetails.weaponClipAmmoCapacity);
				break;

			case WeaponStats.weaponReloadTime:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.weaponReloadTime *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.weaponReloadTime += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.weaponReloadTime = (int)Utilities.ApplyPercentage(value, weaponDetails.weaponReloadTime);
				break;

			case WeaponStats.burstInterval:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.burstInterval *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.burstInterval += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.burstInterval = (int)Utilities.ApplyPercentage(value, weaponDetails.burstInterval);
				break;

			case WeaponStats.fireRate:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.fireRate *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.fireRate += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.fireRate = (int)Utilities.ApplyPercentage(value, weaponDetails.fireRate);
				break;

		}
	}

	public void EnableNewStat(WeaponStats weaponStat, bool toggle)
	{
		switch (weaponStat)
		{
			case WeaponStats.spreadShot:
				weaponDetails.spreadShot = toggle;
				break;

			case WeaponStats.hasInfiniteAmmo:
				weaponDetails.hasInfiniteAmmo = toggle;
				break;

			case WeaponStats.hasInfiniteClipCapacity:
				weaponDetails.hasInfiniteClipCapacity = toggle;
				break;

			case WeaponStats.burstFire:
				weaponDetails.burstFire = toggle;
				break;
		}
	}

	public void UpgradeAmmo(AmmoStats statType, float value, UpgradeAction upgradeAction)
	{
		switch (statType)
		{
			case AmmoStats.AmmoSpeed:
				if (upgradeAction == UpgradeAction.Add)
					ammoDetails.ammoSpeed += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					ammoDetails.ammoSpeed *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					ammoDetails.ammoSpeed = Utilities.ApplyPercentage(value, ammoDetails.ammoSpeed);
				break;

			case AmmoStats.AmmoDamage:
				if (upgradeAction == UpgradeAction.Add)
					ammoDetails.ammoDamage += (int)value;
				else if (upgradeAction == UpgradeAction.Multiply)
					ammoDetails.ammoDamage = (int)(ammoDetails.ammoDamage * value);
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					ammoDetails.ammoDamage = (int)Utilities.ApplyPercentage(value, ammoDetails.ammoDamage);
				break;

			case AmmoStats.AmmoRange:
				if (upgradeAction == UpgradeAction.Add)
					ammoDetails.ammoRange += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					ammoDetails.ammoRange *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					ammoDetails.ammoRange = Utilities.ApplyPercentage(value, ammoDetails.ammoRange);
				break;

			case AmmoStats.AmmoPerShot:
				if (upgradeAction == UpgradeAction.Add)
					ammoDetails.ammoPerShot += (int)value;
				else if (upgradeAction == UpgradeAction.Multiply)
					ammoDetails.ammoPerShot = (int)(ammoDetails.ammoPerShot * value);
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					ammoDetails.ammoPerShot = (int)Utilities.ApplyPercentage(value, ammoDetails.ammoPerShot);
				break;

			case AmmoStats.AmmoShootAngle:
				if (upgradeAction == UpgradeAction.Add)
					ammoDetails.ammoShootAngle += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					ammoDetails.ammoShootAngle *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					ammoDetails.ammoShootAngle = Utilities.ApplyPercentage(value, ammoDetails.ammoShootAngle);
				break;
		}
	}
}
