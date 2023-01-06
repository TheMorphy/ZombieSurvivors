using System.Collections.Generic;
using UnityEngine;

#region REQUIRE COMPONENTS
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
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
[RequireComponent(typeof(SquadControl))]
[DisallowMultipleComponent]
#endregion REQUIRE COMPONENTS

public class Player : MonoBehaviour
{
	[HideInInspector] public PlayerDetailsSO playerDetails;
	[HideInInspector] public HealthEvent healthEvent;
	[HideInInspector] public Health health;
	[HideInInspector] public DestroyedEvent destroyedEvent;
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

	[HideInInspector] public Weapon playerWeapon;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
		health = GetComponent<Health>();
		destroyedEvent = GetComponent<DestroyedEvent>();
		playerController = GetComponent<PlayerController>();
		fireWeaponEvent = GetComponent<FireWeaponEvent>();
		fireWeapon = GetComponent<FireWeapon>();
		setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		activeWeapon = GetComponent<ActiveWeapon>();
		weaponFiredEvent = GetComponent<WeaponFiredEvent>();
		reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
		weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
		animatePlayer = GetComponent<AnimatePlayer>();
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		// Subscribe to player events
		healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
	}

	private void OnDisable()
	{
		// Unsubscribe from player events
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
			//destroyedEvent.CallDestroyedEvent(true ,0);
		}
	}

	/// <summary>
	/// Initialize the player
	/// </summary>
	public void Initialize(PlayerDetailsSO playerDetails)
	{
		this.playerDetails = playerDetails;

		//Create player starting weapons
		CreatePlayerStartingWeapons();

		// Set player starting health
		SetPlayerHealth();
	}

	private void CreatePlayerStartingWeapons()
	{
		AddWeaponToPlayer(playerDetails.startingWeaponDetails);
	}

	/// <summary>
	/// Add a weapon to the player weapon dictionary
	/// </summary>
	public void AddWeaponToPlayer(WeaponDetailsSO weaponDetails)
	{
		playerWeapon = new Weapon() 
		{ 
			weaponDetails = weaponDetails
		};
	}

	/// <summary>
	/// Set player health from playerDetails SO
	/// </summary>
	private void SetPlayerHealth()
	{
		health.SetStartingHealth(playerDetails.Health);
	}
}
