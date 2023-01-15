using UnityEngine;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class Player : MonoBehaviour
{
	[HideInInspector] public PlayerDetailsSO playerDetails;
	[HideInInspector] public PlayerController playerController;
	[HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
	[HideInInspector] public HealthEvent healthEvent;
	[HideInInspector] public SquadControl squadControl;
	[HideInInspector] public Health health;

	[HideInInspector] public Weapon playerWeapon;

	private void Awake()
	{
		playerController = GetComponent<PlayerController>();
		setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		health = GetComponent<Health>();
		healthEvent = GetComponent<HealthEvent>();
		squadControl = GetComponent<SquadControl>();
	}

	private void OnEnable()
	{
		UpgradesManager.OnWeaponUpgrade += UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade += UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade += UpgradesManager_OnAmmoUpgrade;

		healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged1;
	}

	private void HealthEvent_OnHealthChanged1(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		if (healthEventArgs.healthAmount <= 0f)
		{
			playerController.SetPlayerToDead();
		}
	}

	private void OnDisable()
	{
		UpgradesManager.OnWeaponUpgrade -= UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade -= UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade -= UpgradesManager_OnAmmoUpgrade;
	}

	private void UpgradesManager_OnAmmoUpgrade(AmmoUpgradeEventArgs ammoUpgradeEventArgs)
	{
		playerWeapon.UpgradeAmmo(ammoUpgradeEventArgs.ammoStats, ammoUpgradeEventArgs.floatValue, ammoUpgradeEventArgs.upgradeAction);
	}

	private void UpgradesManager_OnPlayerStatUpgrade(PlayerStatUpgradeEventArgs playerStatUpgradeEventArgs)
	{
		if(playerStatUpgradeEventArgs.playerStats == PlayerStats.Health)
		{
			health.UpgradPlayerHealth(playerStatUpgradeEventArgs.floatValue, playerStatUpgradeEventArgs.upgradeAction);
		}
		else if (playerStatUpgradeEventArgs.playerStats == PlayerStats.MoveSpeed)
		{
			playerController.UpgradeMoveSpeed(playerStatUpgradeEventArgs.floatValue, playerStatUpgradeEventArgs.upgradeAction);
		}
	}

	private void UpgradesManager_OnWeaponUpgrade(WeaponUpgradeEventArgs weaponUpgradeEventArgs)
	{
		playerWeapon.UpgradeWeapon(weaponUpgradeEventArgs.weaponStats, weaponUpgradeEventArgs.floatValue, weaponUpgradeEventArgs.boolValue, weaponUpgradeEventArgs.upgradeAction);
	}

	/// <summary>
	/// Initialize the player
	/// </summary>
	public void Initialize(PlayerDetailsSO playerDetails)
	{
		this.playerDetails = Instantiate(playerDetails);

		//Create player starting weapons
		CreatePlayerStartingWeapons();
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
}
