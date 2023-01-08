using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
	[SerializeField] private Transform weaponShootPositionTransform;

	private SetActiveWeaponEvent setWeaponEvent;
	private Weapon currentWeapon;

	private void Awake()
	{
		// Load components
		setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
	}

	private void OnEnable()
	{
		setWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;

		UpgradesManager.Instance.OnWeaponUpgrade += Instance_OnWeaponUpgrade;
		UpgradesManager.Instance.OnAmmoUpgrade += Instance_OnAmmoUpgrade;
	}

	private void OnDisable()
	{
		setWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;

		UpgradesManager.Instance.OnWeaponUpgrade -= Instance_OnWeaponUpgrade;
		UpgradesManager.Instance.OnAmmoUpgrade -= Instance_OnAmmoUpgrade;
	}

	private void Instance_OnWeaponUpgrade(UpgradesManager arg1, WeaponUpgradeEventArgs weaponUpgradeEventArgs)
	{
		currentWeapon.UpgradeWeapon(
			weaponUpgradeEventArgs.weaponStats,
			weaponUpgradeEventArgs.floatValue,
			weaponUpgradeEventArgs.upgradeAction);
	}

	private void Instance_OnAmmoUpgrade(UpgradesManager arg1, AmmoUpgradeEventArgs ammoUpgradeEventArgs)
	{
		currentWeapon.UpgradeAmmo(ammoUpgradeEventArgs.ammoStats, ammoUpgradeEventArgs.floatValue, ammoUpgradeEventArgs.upgradeAction);
	}

	private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
	{
		SetWeapon(setActiveWeaponEventArgs.weapon);
	}

	private void SetWeapon(Weapon weapon)
	{
		currentWeapon = weapon;

		currentWeapon.ammoDetails = weapon.weaponDetails.AmmoDetails;

		// Set weapon shoot position
		weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.WeaponShootPosition;
	}

	public Weapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public AmmoDetailsSO GetCurrentAmmo()
	{
		return currentWeapon.ammoDetails;
	}

	public Vector3 GetShootPosition()
	{
		return weaponShootPositionTransform.position;
	}

	public Transform GetShootFirePointTransform()
	{
		return weaponShootPositionTransform;
	}

	public void RemoveCurrentWeapon()
	{
		currentWeapon = null;
	}
}
