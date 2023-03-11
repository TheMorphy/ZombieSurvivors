public class Weapon
{
	public WeaponDetailsSO weaponDetails;

	public float weaponReloadTimer;
	public int weaponClipRemainingAmmo;
	public int weaponRemainingAmmo;
	public bool isWeaponReloading;

	public void UpgradeWeapon(WeaponStats weaponStat, float value, bool boolValue, UpgradeAction upgradeAction)
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

			case WeaponStats.spreadShot:
				weaponDetails.spreadShot = boolValue;
				break;

			case WeaponStats.hasInfiniteAmmo:
				weaponDetails.hasInfiniteAmmo = boolValue;
				break;

			case WeaponStats.hasInfiniteClipCapacity:
				weaponDetails.hasInfiniteClipCapacity = boolValue;
				break;

			case WeaponStats.burstFire:
				weaponDetails.burstFire = boolValue;
				break;

		}
	}

	public void UpgradeAmmo(AmmoStats statType, float value, UpgradeAction upgradeAction)
	{
		switch (statType)
		{
			case AmmoStats.AmmoSpeed:
				if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoDetails.ammoSpeed += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoDetails.ammoSpeed *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoDetails.ammoSpeed = Utilities.ApplyPercentage(value, weaponDetails.AmmoDetails.ammoSpeed);
				break;

			case AmmoStats.AmmoDamage:
				if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoDetails.ammoDamage += (int)value;
				else if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoDetails.ammoDamage = (int)(weaponDetails.AmmoDetails.ammoDamage * value);
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoDetails.ammoDamage = (int)Utilities.ApplyPercentage(value, weaponDetails.AmmoDetails.ammoDamage);
				break;

			case AmmoStats.AmmoRange:
				if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoDetails.ammoRange += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoDetails.ammoRange *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoDetails.ammoRange = Utilities.ApplyPercentage(value, weaponDetails.AmmoDetails.ammoRange);
				break;

			case AmmoStats.AmmoPerShot:
				if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoDetails.ammoPerShot += (int)value;
				else if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoDetails.ammoPerShot = (int)(weaponDetails.AmmoDetails.ammoPerShot * value);
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoDetails.ammoPerShot = (int)Utilities.ApplyPercentage(value, weaponDetails.AmmoDetails.ammoPerShot);
				break;

			case AmmoStats.AmmoSpread:
				if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoDetails.ammoSpread += value;
				else if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoDetails.ammoSpread *= value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoDetails.ammoSpread = Utilities.ApplyPercentage(value, weaponDetails.AmmoDetails.ammoSpread);
				break;
		}
	}
}
