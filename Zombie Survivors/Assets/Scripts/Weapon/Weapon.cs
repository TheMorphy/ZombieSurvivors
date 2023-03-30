public class Weapon
{
	public WeaponDetailsSO weaponDetails;

	public float weaponReloadTimer;
	public int weaponClipRemainingAmmo;
	public int weaponRemainingAmmo;
	public bool isWeaponReloading;
	public bool WeaponDisabled = true;

	public void UpgradeWeapon(WeaponStats weaponStat, float value, bool boolValue, UpgradeAction upgradeAction)
	{
		switch (weaponStat)
		{
			case WeaponStats.weaponAmmoCapacity:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.AmmoCapacity *= (int)value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.AmmoCapacity += (int)value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.AmmoCapacity = (int)Utilities.ApplyPercentage(value, weaponDetails.AmmoCapacity);
				break;

			case WeaponStats.weaponClipAmmoCapacity:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.MagazineSize *= (int)value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.MagazineSize += (int)value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.MagazineSize = (int)Utilities.ApplyPercentage(value, weaponDetails.MagazineSize);
				break;

			case WeaponStats.weaponReloadTime:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.ReloadTime *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.ReloadTime += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.ReloadTime = (int)Utilities.ApplyPercentage(value, weaponDetails.ReloadTime);
				break;

			case WeaponStats.burstInterval:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.BurstInterval *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.BurstInterval += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.BurstInterval = (int)Utilities.ApplyPercentage(value, weaponDetails.BurstInterval);
				break;

			case WeaponStats.fireRate:
				if (upgradeAction == UpgradeAction.Multiply)
					weaponDetails.FireRate *= value;
				else if (upgradeAction == UpgradeAction.Add)
					weaponDetails.FireRate += value;
				else if (upgradeAction == UpgradeAction.Increase_Percentage)
					weaponDetails.FireRate = (int)Utilities.ApplyPercentage(value, weaponDetails.FireRate);
				break;

			case WeaponStats.spreadShot:
				weaponDetails.SpreadShot = boolValue;
				break;

			case WeaponStats.hasInfiniteAmmo:
				weaponDetails.InfiniteAmmo = boolValue;
				break;

			case WeaponStats.hasInfiniteClipCapacity:
				weaponDetails.InfiniteMagazineCapacity = boolValue;
				break;

			case WeaponStats.burstFire:
				weaponDetails.BurstFire = boolValue;
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
