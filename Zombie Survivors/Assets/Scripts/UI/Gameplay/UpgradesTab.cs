using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradesTab : Tab
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

	public override void Initialize(object[] args = null)
	{
		SetLevelSystem(GameManager.Instance.GetLevelSystem());
	}

	private void InitializeUpgradeCards()
	{
		for (int i = 0; i < upgradeCards.Length; i++)
		{
			upgradeType = Utilities.GetRandomEnumValue<UpgradeType>();

			switch (upgradeType)
			{
				case UpgradeType.WeaponUpgrade:
					
					var weaponStatsUpgradesList = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.WeaponUpgrades;
					int randomWeaponStatsUpgradesListIndex = UnityEngine.Random.Range(0, weaponStatsUpgradesList.Count);

					statName = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].WeaponStats.ToString();
					tempText = Utilities.GetDescription<WeaponStats>(statName);

					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.WeaponPicture;

					toggleValue = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].Toggle;
					upgradeAction = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].UpgradeAction;
					statValue = weaponStatsUpgradesList[randomWeaponStatsUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.AmmoUpgrade:

					var ammoUpgradesList = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoUpgrades;
					int randomAmmoUpgradesListIndex = UnityEngine.Random.Range(0, ammoUpgradesList.Count);

					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoPicture;

					statName = ammoUpgradesList[randomAmmoUpgradesListIndex].AmmoStats.ToString();
					tempText = Utilities.GetDescription<AmmoStats>(statName);

					toggleValue = ammoUpgradesList[randomAmmoUpgradesListIndex].Toggle;
					upgradeAction = ammoUpgradesList[randomAmmoUpgradesListIndex].UpgradeAction;
					statValue = ammoUpgradesList[randomAmmoUpgradesListIndex].FloatValue;
					break;

				case UpgradeType.PlayerStatUpgrade:

					var playerStatsUpgradesList = GameManager.Instance.GetPlayer().PlayerDetails.PlayerStatsUpgrades;
					int randomplayerStatsUpgradesListIndex = UnityEngine.Random.Range(0, playerStatsUpgradesList.Count);

					statName = playerStatsUpgradesList[randomplayerStatsUpgradesListIndex].PlayerStats.ToString();
					tempText = Utilities.GetDescription<PlayerStats>(statName);

					tempImg = GameManager.Instance.GetPlayer().PlayerWeapon.weaponDetails.AmmoDetails.AmmoPicture;

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

	public void CallCardSelectedEvent()
	{
		Hide();
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
		levelNumberText.text = (levelNumber + 1).ToString();
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
