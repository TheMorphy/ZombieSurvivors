using UnityEngine;

public class UpgradeCard : MonoBehaviour
{
	private float cardFloatValue;
	private bool cardBoolValue;
	private int cardIntValue;
	private string statType;
	private UpgradeAction upgradeAction;

	private UpgradeType upgradeType;

	private UpgradesUI levelUI;

	private void Awake()
	{
		levelUI = GetComponentInParent<UpgradesUI>();
	}

	// Initializes card with FLOAT value
	public void InitializeCard(UpgradeType upgradeType, string statType, float cardFloatValue, bool cardBoolValue, UpgradeAction upgradeAction)
	{
		this.cardFloatValue = cardFloatValue;
		this.upgradeType = upgradeType;
		this.statType = statType;
		this.upgradeAction = upgradeAction;
		this.cardBoolValue = cardBoolValue;
	}

	// Initializes card with INT value
	public void InitializeCard(UpgradeType upgradeType, string statType, int cardIntValue, bool cardBoolValue, UpgradeAction upgradeAction)
	{
		this.cardIntValue = cardIntValue;
		this.upgradeType = upgradeType;
		this.statType = statType;
		this.upgradeAction = upgradeAction;
		this.cardBoolValue = cardBoolValue;
	}

	public void UpgradeAction()
	{
		switch (upgradeType)
		{
			case UpgradeType.WeaponUpgrade:

				WeaponStats weaponStat = Utilities.GetEnumValue<WeaponStats>(statType);

				UpgradesManager.UpgradeWeapon(weaponStat, upgradeAction, cardFloatValue, cardIntValue, cardBoolValue);

				break;

			case UpgradeType.AmmoUpgrade:

				AmmoStats ammoStat = Utilities.GetEnumValue<AmmoStats>(statType);

				UpgradesManager.UpgradeAmmo(ammoStat, upgradeAction, cardFloatValue, cardIntValue, cardBoolValue);

				break;

			case UpgradeType.PlayerStatUpgrade:

				PlayerStats playerStat = Utilities.GetEnumValue<PlayerStats>(statType);

				UpgradesManager.UpgradePlayerStat(playerStat, upgradeAction, cardFloatValue, cardIntValue, cardBoolValue);

				break;
		}

		levelUI.CallCardSelectedEvent();
	}
}
