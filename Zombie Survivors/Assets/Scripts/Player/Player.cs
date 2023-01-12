using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

#region REQUIRE COMPONENTS
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimatePlayer))]
[DisallowMultipleComponent]
#endregion REQUIRE COMPONENTS

public class Player : MonoBehaviour
{
	[HideInInspector] public PlayerDetailsSO playerDetails;
	[HideInInspector] public HealthEvent healthEvent;
	[HideInInspector] public Health health;
	[HideInInspector] public PlayerController playerController;
	[HideInInspector] public FireWeaponEvent fireWeaponEvent;
	[HideInInspector] public FireWeapon fireWeapon;
	[HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
	[HideInInspector] public ActiveWeapon activeWeapon;
	[HideInInspector] public WeaponFiredEvent weaponFiredEvent;
	[HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
	[HideInInspector] public WeaponReloadedEvent weaponReloadedEvent;
	[HideInInspector] public AnimatePlayer animatePlayer;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Transform parent;

	[HideInInspector] public Weapon playerWeapon;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
		health = GetComponent<Health>();
		playerController = GetComponent<PlayerController>();
		fireWeaponEvent = GetComponent<FireWeaponEvent>();
		fireWeapon = GetComponent<FireWeapon>();
		setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		activeWeapon = GetComponent<ActiveWeapon>();
		weaponFiredEvent = GetComponent<WeaponFiredEvent>();
		reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
		weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
		parent = GetComponentInParent<Transform>();
		animatePlayer = GetComponent<AnimatePlayer>();
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		SquadControl.ComradesTransforms.Add(transform);
	}

	private void OnEnable()
	{
		UpgradesManager.OnWeaponUpgrade += UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade += UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade += UpgradesManager_OnAmmoUpgrade;

		healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
	}

	private void OnDisable()
	{
		UpgradesManager.OnWeaponUpgrade -= UpgradesManager_OnWeaponUpgrade;

		UpgradesManager.OnPlayerStatUpgrade -= UpgradesManager_OnPlayerStatUpgrade;

		UpgradesManager.OnAmmoUpgrade -= UpgradesManager_OnAmmoUpgrade;

		healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
	}

	/// <summary>
	/// Handle health changed event
	/// </summary>
	private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		// If player has died
		if (healthEventArgs.healthAmount <= 0f)
		{
			animator.SetTrigger("Die");
		}
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

		// Set player starting health
		SetPlayerHealth();
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

	/// <summary>
	/// Set player health from playerDetails SO
	/// </summary>
	private void SetPlayerHealth()
	{
		health.SetStartingHealth(playerDetails.Health);
	}
}
