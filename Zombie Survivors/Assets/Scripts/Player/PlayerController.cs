using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	private Player player;
	private bool isPlayerMovementDisabled = false;

	[SerializeField] private Transform Torso;
	[SerializeField] private float turnToEnemyDistance = 6f;

	[HideInInspector] public FixedJoystick joystick;

	private void Awake()
	{
		joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FixedJoystick>(); // I no like dis, but it works for now

		rb = GetComponent<Rigidbody>();
		player = GetComponent<Player>();
	}
	private void Start()
	{
		SetStartingWeapon();
	}
	private void Update()
	{
		WeaponInput();
		HandleRotations();
	}

	private void FixedUpdate()
	{
		if (isPlayerMovementDisabled)
			return;

		HandleMovement();
	}

	/// <summary>
	/// Set the player starting weapon
	/// </summary>
	private void SetStartingWeapon()
	{
		player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.playerWeapon);
	}
	private void HandleMovement()
	{
		rb.velocity = new Vector3(joystick.Horizontal * player.playerDetails.MoveSpeed, rb.velocity.y, joystick.Vertical * player.playerDetails.MoveSpeed);

		if (joystick.Horizontal != 0 || joystick.Vertical != 0)
		{
			transform.rotation = Quaternion.LookRotation(rb.velocity);
		}
	}

	private void HandleRotations()
	{
		var target = GetClosestEnemy(EnemySpawner.activeEnemies);

		if (target != null)
		{
			if ((target.position - transform.position).magnitude < turnToEnemyDistance)
			{
				Torso.LookAt(target);
			}
			else
			{
				Torso.localRotation = Quaternion.Euler(0, 0, 0);
			}
		}
	}

	Transform GetClosestEnemy(List<Transform> enemies)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		for (int i = 0; i < enemies.Count; i++)
		{
			Vector3 directionToTarget = enemies[i].position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = enemies[i];
			}
		}

		return bestTarget;
	}

	/// <summary>
	/// Weapon Input
	/// </summary>
	private void WeaponInput()
	{
		// Fire weapon input
		FireWeaponInput();

		// Reload weapon input
		ReloadWeaponInput();
	}

	private void FireWeaponInput()
	{
		// Trigger fire weapon event
		player.fireWeaponEvent.CallFireWeaponEvent(true);
	}

	private void ReloadWeaponInput()
	{
		Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

		// if current weapon is reloading return
		if (currentWeapon.isWeaponReloading) return;

		// remaining ammo is less than clip capacity then return and not infinite ammo then return
		if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo) return;

		// if ammo in clip equals clip capacity then return
		if (currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity) return;
	}

	/// <summary>
	/// Enable the player movement
	/// </summary>
	public void EnablePlayer()
	{
		isPlayerMovementDisabled = false;
	}

	/// <summary>
	/// Disable the player movement
	/// </summary>
	public void DisablePlayer()
	{
		isPlayerMovementDisabled = true;
		player.idleEvent.CallIdleEvent();
	}
}
