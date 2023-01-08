using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LevelUI : MonoBehaviour
{
	[SerializeField] private GameObject[] upgradeCards;
	private Image[] cardsImages;
	private TextMeshProUGUI[] cardsDescriptions;

    [SerializeField] private TextMeshProUGUI levelNumberText;
	[SerializeField] private Image experienceBarImage;

	private LevelSystem levelSystem;

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

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void InitializeUpgradeCards()
	{
		for (int i = 0; i < upgradeCards.Length; i++)
		{
			upgradeType = Utilities.GetRandomEnumValue<UpgradeType>();

			switch (upgradeType)
			{
				case UpgradeType.WeaponUpgrade:
					statName = Utilities.GetRandomEnumName<WeaponStats>();
					tempText = Utilities.GetDescription<WeaponStats>(statName);

					var weaponStatsUpgradesList = GameManager.Instance.GetPlayer().activeWeapon.GetCurrentWeapon().weaponDetails.WeaponUpgrades;
					int randomWeaponStatsUpgradesListIndex = UnityEngine.Random.Range(0, weaponStatsUpgradesList.Count);

					tempImg = GameManager.Instance.GetPlayer().activeWeapon.GetCurrentWeapon().weaponDetails.WeaponPicture;

					toggleValue = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].Toggle;
					upgradeAction = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].UpgradeAction;
					statValue = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.AmmoUpgrade:
					statName = Utilities.GetRandomEnumName<AmmoStats>();
					tempText = Utilities.GetDescription<AmmoStats>(statName);

					var ammoUpgradesList = GameManager.Instance.GetPlayer().activeWeapon.GetCurrentAmmo().AmmoUpgrades;
					int randomAmmoUpgradesListIndex = UnityEngine.Random.Range(0, ammoUpgradesList.Count);

					tempImg = GameManager.Instance.GetPlayer().activeWeapon.GetCurrentAmmo().AmmoPicture;

					toggleValue = ammoUpgradesList[randomAmmoUpgradesListIndex].Toggle;
					upgradeAction = ammoUpgradesList[randomAmmoUpgradesListIndex].UpgradeAction;
					statValue = ammoUpgradesList[randomAmmoUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.PlayerStatUpgrade:
					statName = Utilities.GetRandomEnumName<PlayerStats>();
					tempText = Utilities.GetDescription<PlayerStats>(statName);

					var playerStatsUpgradesList = GameManager.Instance.GetPlayer().playerDetails.PlayerStatsUpgrades;
					int randomplayerStatsUpgradesListIndex = UnityEngine.Random.Range(0, playerStatsUpgradesList.Count);

					tempImg = GameManager.Instance.GetPlayer().activeWeapon.GetCurrentAmmo().AmmoPicture;

					toggleValue = playerStatsUpgradesList[randomplayerStatsUpgradesListIndex].Toggle;
					upgradeAction = playerStatsUpgradesList[randomplayerStatsUpgradesListIndex].UpgradeAction;
					statValue = playerStatsUpgradesList[randomplayerStatsUpgradesListIndex].FloatValue;
					break;
			}

			var card = upgradeCards[i].GetComponent<UpgradeCard>();
			card.InitializeCard(upgradeType, statName, statValue, toggleValue, upgradeAction);

			DisplayCardInfo(cardsImages[i], cardsDescriptions[i]);
		}
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

	public void DisableUI()
	{
		gameObject.SetActive(false);
	}

	private void SetExperienceBarSize(float experienceNormalized)
	{
		experienceBarImage.fillAmount = experienceNormalized;
	}

	private void SetLevelNumber(int levelNumber)
	{
		levelNumberText.text = (levelNumber + 1).ToString();
	}

	public void SetLevelSystem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;

		SetLevelNumber(levelSystem.GetPlayerLevel());
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
	}

	private void LevelSystem_OnLevelChanged(object sender, EventArgs eventArgs)
	{
		SetLevelNumber(levelSystem.GetPlayerLevel());

		gameObject.SetActive(true);

		InitializeUpgradeCards();
	}

	private void LevelSystem_OnExperienceChanged(object sender, EventArgs eventArgs)
	{
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());
	}
}
