using System;
using System.Collections.Generic;

[Serializable]
public class Equipment
{
	public Weapon playerWeapon;
	public PlayerDetailsSO playerDetails;

	public event Action OnUpgraded;

	public Equipment(Weapon weapon, PlayerDetailsSO playerDetails)
	{
		playerWeapon = weapon;
		this.playerDetails = playerDetails;
	}

	public void SetUpgrades(List<CardDTO> upgradeCards)
	{
		foreach (CardDTO card in upgradeCards)
		{
			switch (card.CardType)
			{
				case CardType.Weapon:
					UpgradeWeapon(card);
					break;
				case CardType.Boots:
					UpgradeBoots(card);
					break;
				case CardType.Armor:
					UpgradeArmor(card);
					break;
				case CardType.Helmet:
					UpgradeHelmet(card);
					break;
				case CardType.Gloves:
					UpgradeGloves(card);
					break;
			}
		}
		CallPlayerUpgradedEvent();
	}

	public void CallPlayerUpgradedEvent()
	{
		OnUpgraded?.Invoke();
	}

	private void UpgradeWeapon(CardDTO card)
	{
		switch(card.WeaponStat)
		{
			case WeaponStats.fireRate:
				card.Upgrade(ref playerWeapon.weaponDetails.fireRate, card.UpgradeAction);
				break;
			case WeaponStats.burstInterval:
				card.Upgrade(ref playerWeapon.weaponDetails.burstInterval, card.UpgradeAction);
				break;
			case WeaponStats.weaponReloadTime:
				card.Upgrade(ref playerWeapon.weaponDetails.weaponReloadTime, card.UpgradeAction);
				break;
			case WeaponStats.weaponAmmoCapacity:
				card.Upgrade(ref playerWeapon.weaponDetails.weaponAmmoCapacity, card.UpgradeAction);
				break;
			case WeaponStats.weaponClipAmmoCapacity:
				card.Upgrade(ref playerWeapon.weaponDetails.weaponClipAmmoCapacity, card.UpgradeAction);
				break;
		}
	}

	// Gloves increase HP and decrease spread
	// TODO: Decrease spread
	private void UpgradeGloves(CardDTO card)
	{
		switch (card.PlayerStat)
		{
			case PlayerStats.Health:
				card.Upgrade(ref playerDetails.Health, card.UpgradeAction);
				break;
			case PlayerStats.MoveSpeed:
				//card.Upgrade(ref playerDetails.MoveSpeed, card.UpgradeAction);
				break;
		}
	}

	// Helmet increases HP
	private void UpgradeHelmet(CardDTO card)
	{
		switch (card.PlayerStat)
		{
			case PlayerStats.Health:
				card.Upgrade(ref playerDetails.Health, card.UpgradeAction);
				break;
			case PlayerStats.MoveSpeed:
				card.Upgrade(ref playerDetails.MoveSpeed, card.UpgradeAction);
				break;
		}
	}

	// Boots increase Speed and HP
	// TODO: increase HP
	private void UpgradeBoots(CardDTO card)
	{
		switch (card.PlayerStat)
		{
			case PlayerStats.Health:
				//card.Upgrade(ref playerDetails.Health, card.UpgradeAction);
				break;
			case PlayerStats.MoveSpeed:
				card.Upgrade(ref playerDetails.MoveSpeed, card.UpgradeAction);
				break;
		}
	}

	// Armor increases HP but decreases speed
	private void UpgradeArmor(CardDTO card)
    {
		switch (card.PlayerStat)
		{
			case PlayerStats.Health:
				card.Upgrade(ref playerDetails.Health, card.UpgradeAction);
				break;
			case PlayerStats.MoveSpeed:
				//card.Upgrade(ref playerDetails.MoveSpeed, card.UpgradeAction);
				break;
		}
	}
}
