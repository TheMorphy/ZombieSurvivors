using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	private void OnDisable()
	{
		setWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;
	}

	private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
	{
		SetWeapon(setActiveWeaponEventArgs.weapon);
	}

	private void SetWeapon(Weapon weapon)
	{
		currentWeapon = weapon;

		// Set weapon shoot position
		weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.weaponShootPosition;
	}

	public Weapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public AmmoDetailsSO GetCurrentAmmo()
	{
		return currentWeapon.weaponDetails.ammoDetails;
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
