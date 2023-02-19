using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
	[HideInInspector] public PlayerDetailsSO playerDetails;
	[HideInInspector] public PlayerController playerController;
	[HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
	[HideInInspector] public SquadControl squadControl;
	[HideInInspector] public Weapon playerWeapon;

	[Space]
	[Header("Readonly")]
	public List<CardDTO> activeUpgrades;

	[HideInInspector] public Equipment PlayerEquipment;

	private void Awake()
	{
		playerController = GetComponent<PlayerController>();
		setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		squadControl = GetComponent<SquadControl>();
	}

	private void OnEnable()
	{
		StaticEvents.CallPlayerInitializedEvent(transform);

		UpgradesManager.OnWeaponUpgrade += UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade += UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade += UpgradesManager_OnAmmoUpgrade;

		squadControl.OnSquadAmmountChanged += PlayerController_OnSquadIncrease;
	}

	private void OnDisable()
	{
		UpgradesManager.OnWeaponUpgrade -= UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade -= UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade -= UpgradesManager_OnAmmoUpgrade;

		squadControl.OnSquadAmmountChanged -= PlayerController_OnSquadIncrease;

		PlayerEquipment.OnUpgraded -= PlayerEquipment_OnUpgraded;
	}

	private void UpgradesManager_OnAmmoUpgrade(AmmoUpgradeEventArgs ammoUpgradeEventArgs)
	{
		playerWeapon.UpgradeAmmo(ammoUpgradeEventArgs.ammoStats, ammoUpgradeEventArgs.floatValue, ammoUpgradeEventArgs.upgradeAction);
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
			playerController.UpgradeMoveSpeed(playerStatUpgradeEventArgs.floatValue, playerStatUpgradeEventArgs.upgradeAction);
		}
	}

	private void UpgradesManager_OnWeaponUpgrade(WeaponUpgradeEventArgs weaponUpgradeEventArgs)
	{
		playerWeapon.UpgradeWeapon(
			weaponUpgradeEventArgs.weaponStats, 
			weaponUpgradeEventArgs.floatValue, 
			weaponUpgradeEventArgs.boolValue, 
			weaponUpgradeEventArgs.upgradeAction);
	}

	private void PlayerController_OnSquadIncrease(SquadControl squadControl, SquadControlEventArgs squadControlEventArgs)
	{
		if(squadControlEventArgs.squadSize == 0)
		{
			playerController.SetPlayerToDead();
		}

		squadControl.FormatSquad();
	}


	/// <summary>
	/// Initialize the player
	/// </summary>
	public void Initialize(PlayerDetailsSO playerDetails)
	{
		this.playerDetails = Instantiate(playerDetails);

		playerController.enabled = false;
		//Create player starting weapons
		CreatePlayerStartingWeapons();

		ApplyUpgrades();
	}

	private void CreatePlayerStartingWeapons()
	{
		AddWeaponToPlayer(playerDetails.PlayerWeaponDetails);
	}

	/// <summary>
	/// Add a weapon to the player weapon dictionary
	/// </summary>
	public void AddWeaponToPlayer(WeaponDetailsSO weaponWeaponDetails)
	{
		playerWeapon = new Weapon()
		{
			weaponDetails = Instantiate(weaponWeaponDetails),
			weaponReloadTimer = 0f, 
			weaponClipRemainingAmmo = weaponWeaponDetails.weaponClipAmmoCapacity, 
			weaponRemainingAmmo = weaponWeaponDetails.weaponAmmoCapacity, 
			isWeaponReloading = false 
		};

		playerWeapon.weaponDetails.AmmoDetails = Instantiate(weaponWeaponDetails.AmmoDetails);
	}

	private void ApplyUpgrades()
	{
		PlayerEquipment = new Equipment(playerWeapon, playerDetails);

		PlayerEquipment.OnUpgraded += PlayerEquipment_OnUpgraded;

		activeUpgrades = SaveManager.ReadFromJSON<CardDTO>(Settings.CARDS).Where(x => x.CardSlot == CardSlot.Active).ToList();

		PlayerEquipment.SetUpgrades(activeUpgrades);
	}

	private void PlayerEquipment_OnUpgraded()
	{
		playerController.enabled = true;
		squadControl.CreateFirstComrade();

		CameraController.Instance.SetInitialTarget(transform);
	}
}
