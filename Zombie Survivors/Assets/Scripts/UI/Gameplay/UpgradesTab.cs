using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UpgradesTab : Tab
{
	[SerializeField] private GameObject[] upgradeCards;
	private Image[] cardsImages;
	private TextMeshProUGUI[] cardsDescriptions;

    [SerializeField] private TextMeshProUGUI levelNumberText;
	[SerializeField] private Image experienceBarImage;

	private LevelSystem levelSystem;

	private List<WeaponUpgradeDetails> weaponUpgradeDetails;
	private List<PlayerStatsUpgradeDetails> playerStatsUpgradeDetails;
	private List<AmmoUpgradeDetails> ammoUpgradeDetails;

	private UpgradeType upgradeType;
	private UpgradeAction upgradeAction;
	private string statName;
	private float statValue;
	private bool toggleValue;

	Sprite tempImg;
	string tempText;

	private void Awake()
	{
		cardsImages = new Image[upgradeCards.Length];
		cardsDescriptions = new TextMeshProUGUI[upgradeCards.Length];

		for (int i = 0; i < upgradeCards.Length; i++)
		{
			cardsImages[i] = Utilities.GetComponentInChildrenButNotParent<Image>(upgradeCards[i]);
			cardsDescriptions[i] = upgradeCards[i].GetComponentInChildren<TextMeshProUGUI>();
		}
	}

	public override void Initialize(object[] args = null)
	{
		SetLevelSystem(GameManager.Instance.GetLevelSystem());
	}

	private void InitializeUpgradeCards()
	{
		weaponUpgradeDetails = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.WeaponUpgrades;
		ammoUpgradeDetails = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoUpgrades;
		playerStatsUpgradeDetails = GameManager.Instance.GetPlayer().PlayerDetails.PlayerStatsUpgrades;

		List<UpgradeType> usedUpgrades = new List<UpgradeType>();

		for (int i = 0; i < upgradeCards.Length; i++)
		{
			do
			{
				upgradeType = Utilities.GetRandomEnumValue<UpgradeType>();
			}
			while (usedUpgrades.Contains(upgradeType));
			usedUpgrades.Add(upgradeType);

			switch (upgradeType)
			{
				case UpgradeType.WeaponUpgrade:
					
					int randomWeaponStatsUpgradesListIndex = UnityEngine.Random.Range(0, weaponUpgradeDetails.Count);

					statName = weaponUpgradeDetails[randomWeaponStatsUpgradesListIndex].WeaponStats.ToString();
					tempText = Utilities.GetDescription<WeaponStats>(statName);

					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.WeaponPicture;

					toggleValue = weaponUpgradeDetails[randomWeaponStatsUpgradesListIndex].Toggle;
					upgradeAction = weaponUpgradeDetails[randomWeaponStatsUpgradesListIndex].UpgradeAction;
					statValue = weaponUpgradeDetails[randomWeaponStatsUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.AmmoUpgrade:

					int randomAmmoUpgradesListIndex = UnityEngine.Random.Range(0, ammoUpgradeDetails.Count);
					
					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoPicture;

					statName = ammoUpgradeDetails[randomAmmoUpgradesListIndex].AmmoStats.ToString();
					tempText = Utilities.GetDescription<AmmoStats>(statName);

					toggleValue = ammoUpgradeDetails[randomAmmoUpgradesListIndex].Toggle;
					upgradeAction = ammoUpgradeDetails[randomAmmoUpgradesListIndex].UpgradeAction;
					statValue = ammoUpgradeDetails[randomAmmoUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.PlayerStatUpgrade:

					int randomplayerStatsUpgradesListIndex = UnityEngine.Random.Range(0, playerStatsUpgradeDetails.Count);

					statName = playerStatsUpgradeDetails[randomplayerStatsUpgradesListIndex].PlayerStats.ToString();
					tempText = Utilities.GetDescription<PlayerStats>(statName);

					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoPicture;

					toggleValue = playerStatsUpgradeDetails[randomplayerStatsUpgradesListIndex].Toggle;
					upgradeAction = playerStatsUpgradeDetails[randomplayerStatsUpgradesListIndex].UpgradeAction;
					statValue = playerStatsUpgradeDetails[randomplayerStatsUpgradesListIndex].FloatValue;
					break;
			}

			var card = upgradeCards[i].GetComponent<UpgradeCard>();
			card.InitializeCard(upgradeType, statName, statValue, toggleValue, upgradeAction);

			DisplayCardInfo(cardsImages[i], cardsDescriptions[i]);
		}
	}

	public void CallCardSelectedEvent()
	{
		Hide();
		levelSystem.LeveledUp = false;
		GameManager.Instance.GetPlayer().PlayerController.EnablePlayerMovement();
	}

	private void DisplayCardInfo(Image cardImage, TextMeshProUGUI cardDescription)
	{
		cardImage.sprite = tempImg;

		if (upgradeAction == UpgradeAction.Add)
		{
			cardDescription.text = $"Add {statValue} to {tempText}";
		}
		else if (upgradeAction == UpgradeAction.Multiply)
		{
			cardDescription.text = $"Multiply {tempText} by {statValue}";
		}
		else if (upgradeAction == UpgradeAction.Toggle)
		{
			if(toggleValue == true) 
			{
				cardDescription.text = $"Enable {tempText}";
			}
			else
			{
				cardDescription.text = $"Disable {tempText}";
			}
		}
		else if (upgradeAction == UpgradeAction.Increase_Percentage)
		{
			cardDescription.text = $"Increase {tempText} by {statValue}%";
		}
	}

	private void SetExperienceBarSize(float experienceNormalized)
	{
		experienceBarImage.fillAmount = experienceNormalized;
	}

	private void SetLevelNumber(int levelNumber)
	{
		levelNumberText.text = levelNumber.ToString();
	}

	public void SetLevelSystem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;

		SetLevelNumber(levelSystem.GetPlayerLevel());
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());

		levelSystem.OnLevelUp += LevelSystem_OnLevelChanged;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;

		Hide();
	}

	private void LevelSystem_OnLevelChanged(object sender, EventArgs eventArgs)
	{
		GameManager.Instance.GetPlayer().PlayerController.DisablePlayerMovement();

		Show();
		SetLevelNumber(levelSystem.GetPlayerLevel());
		InitializeUpgradeCards();
	}

	private void LevelSystem_OnExperienceChanged(object sender, EventArgs eventArgs)
	{
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());
	}
}
