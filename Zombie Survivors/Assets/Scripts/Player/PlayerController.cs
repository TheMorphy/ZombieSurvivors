using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	private Player player;
	private bool isPlayerMovementDisabled = false;
	private Quaternion currentRotation;

	[SerializeField] private Transform Torso;

	[HideInInspector] private FloatingJoystick joystick;


	private void Awake()
	{
		joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystick>(); // I no like dis, but it works for now

		rb = GetComponent<Rigidbody>();
		player = GetComponent<Player>();
	}
	private void Start()
	{
		SetStartingWeapon();
	}

	private void Update()
	{
		if(!isPlayerMovementDisabled)
		{
			WeaponInput();
		}
	}

	private void FixedUpdate()
	{
		if (isPlayerMovementDisabled)
			return;

		HandleMovement();
	}

	private void HandleMovement()
	{
		rb.velocity = new Vector3(joystick.Horizontal * player.playerDetails.MoveSpeed, rb.velocity.y, joystick.Vertical * player.playerDetails.MoveSpeed);

		if (joystick.Horizontal != 0 || joystick.Vertical != 0)
		{
			transform.rotation = Quaternion.LookRotation(rb.velocity);
		}

		HandleRotations();
	}

	private void HandleRotations()
	{
		var target = GetClosestEnemy(EnemySpawner.activeEnemies);

		if (target != null)
		{
			Torso.LookAt(target);
		}
		else
		{
			Torso.localRotation = Quaternion.Euler(0, 0, 0);
		}
	}

	private void SetStartingWeapon()
	{
		player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.playerWeapon);
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
		player.fireWeapon.FireWeapn();

		// Reload weapon input
		ReloadWeaponInput();
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

	public void EnablePlayerMovement()
	{
		joystick.gameObject.SetActive(true);

		rb.velocity = Vector3.zero;

		transform.rotation = currentRotation;

		isPlayerMovementDisabled = false;

		Time.timeScale = 1;
	}

	public void DisablePlayerMovement()
	{
		joystick.transform.GetChild(0).gameObject.SetActive(false);

		rb.velocity = Vector3.zero;

		currentRotation = transform.rotation;

		joystick.gameObject.SetActive(false);

		isPlayerMovementDisabled = true;

		Time.timeScale = 0;

		joystick.ResetJoystick();
	}

	public void UpgradeMoveSpeed(float value, UpgradeAction upgradeAction)
	{
		if (upgradeAction == UpgradeAction.Add)
		{
			player.playerDetails.MoveSpeed += value;
		}
		else if (upgradeAction == UpgradeAction.Multiply)
		{
			player.playerDetails.MoveSpeed *= value;
		}
		else if (upgradeAction == UpgradeAction.Increase_Percentage)
		{
			player.playerDetails.MoveSpeed = Utilities.ApplyPercentage(value, player.playerDetails.MoveSpeed);
		}
	}

}
