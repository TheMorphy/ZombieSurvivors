using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
	[HideInInspector] public PlayerDetailsSO PlayerDetails;
	[HideInInspector] public PlayerController PlayerController;
	[HideInInspector] public SquadControl SquadControl;
	[HideInInspector] public Weapon PlayerWeapon;

	[Space]
	[Header("Readonly")]
	// For debugging only. Can be deleted
	public List<CardDTO> activeUpgrades;

	[HideInInspector] public Equipment PlayerEquipment;

	private void Awake()
	{
		PlayerController = GetComponent<PlayerController>();
		SquadControl = GetComponent<SquadControl>();
	}

	private void OnEnable()
	{
		StaticEvents.CallPlayerInitializedEvent(transform);

		UpgradesManager.OnWeaponUpgrade += UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade += UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade += UpgradesManager_OnAmmoUpgrade;

		SquadControl.OnSquadAmmountChanged += PlayerController_OnSquadIncrease;
	}

	private void OnDisable()
	{
		UpgradesManager.OnWeaponUpgrade -= UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade -= UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade -= UpgradesManager_OnAmmoUpgrade;

		SquadControl.OnSquadAmmountChanged -= PlayerController_OnSquadIncrease;

		PlayerEquipment.OnUpgraded -= PlayerEquipment_OnUpgraded;
	}

	private void UpgradesManager_OnAmmoUpgrade(AmmoUpgradeEventArgs ammoUpgradeEventArgs)
	{
		PlayerWeapon.UpgradeAmmo(ammoUpgradeEventArgs.ammoStats, ammoUpgradeEventArgs.floatValue, ammoUpgradeEventArgs.upgradeAction);
	}

	private void UpgradesManager_OnPlayerStatUpgrade(PlayerStatUpgradeEventArgs playerStatUpgradeEventArgs)
	{
		if(playerStatUpgradeEventArgs.playerStats == PlayerStats.Health)
		{
			SquadControl.ComradesTransforms
				.ForEach(x => x.GetComponent<Health>()
				.UpgradPlayerHealth(playerStatUpgradeEventArgs.floatValue, playerStatUpgradeEventArgs.upgradeAction));
		}
		else if (playerStatUpgradeEventArgs.playerStats == PlayerStats.MoveSpeed)
		{
			PlayerController.UpgradeMoveSpeed(playerStatUpgradeEventArgs.floatValue, playerStatUpgradeEventArgs.upgradeAction);
		}
	}

	private void UpgradesManager_OnWeaponUpgrade(WeaponUpgradeEventArgs weaponUpgradeEventArgs)
	{
		PlayerWeapon.UpgradeWeapon(
			weaponUpgradeEventArgs.weaponStats, 
			weaponUpgradeEventArgs.floatValue, 
			weaponUpgradeEventArgs.boolValue, 
			weaponUpgradeEventArgs.upgradeAction);
	}

	private void PlayerController_OnSquadIncrease(SquadControl squadControl, SquadControlEventArgs squadControlEventArgs)
	{
		if(squadControlEventArgs.squadSize == 0)
		{
			PlayerController.SetPlayerToDead();
			GameManager.Instance.CallGameStateChangedEvent(GameState.gameLost);
			return;
		}

		squadControl.FormatSquad();
	}

	/// <summary>
	/// Initialize the player
	/// </summary>
	public void Initialize(PlayerDetailsSO playerDetails)
	{
		SquadControl.ComradesTransforms = new List<Transform>();

		this.PlayerDetails = Instantiate(playerDetails);

		PlayerController.enabled = false;

		AddWeaponToPlayer(PlayerDetails.PlayerWeaponDetails);

		ApplyUpgrades();
	}

	/// <summary>
	/// Add a weapon to the player
	/// </summary>
	public void AddWeaponToPlayer(WeaponDetailsSO weaponWeaponDetails)
	{
		PlayerWeapon = new Weapon()
		{
			weaponDetails = Instantiate(weaponWeaponDetails),
			weaponReloadTimer = 0f, 
			weaponClipRemainingAmmo = weaponWeaponDetails.weaponClipAmmoCapacity, 
			weaponRemainingAmmo = weaponWeaponDetails.weaponAmmoCapacity, 
			isWeaponReloading = false 
		};

		PlayerWeapon.weaponDetails.AmmoDetails = Instantiate(weaponWeaponDetails.AmmoDetails);
	}

	/// <summary>
	/// Apply selected upgrade cards values to the player weapon and base stats
	/// </summary>
	private void ApplyUpgrades()
	{
		PlayerEquipment = new Equipment(PlayerWeapon, PlayerDetails);

		PlayerEquipment.OnUpgraded += PlayerEquipment_OnUpgraded;

		activeUpgrades = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS).Where(x => x.CardSlot == CardSlot.Active).ToList();

		if (activeUpgrades.Count > 0)
			PlayerEquipment.SetUpgrades(activeUpgrades);
		else
			PlayerEquipment_OnUpgraded();
	}

	/// <summary>
	/// Call actions after the upgrade cards have been added to the player
	/// </summary>
	private void PlayerEquipment_OnUpgraded()
	{
		PlayerController.enabled = true;
		SquadControl.CreateFirstComrade();

		CameraController.Instance.SetInitialTarget(transform);
	}
}
