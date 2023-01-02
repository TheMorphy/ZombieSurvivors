using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
	private ActiveWeapon activeWeapon;
	private FireWeaponEvent fireWeaponEvent;
	private ReloadWeaponEvent reloadWeaponEvent;

	[SerializeField]
	private LayerMask Mask;

	private float LastShootTime;

	private void Awake()
	{
		// Load components.
		activeWeapon = GetComponent<ActiveWeapon>();
		fireWeaponEvent = GetComponent<FireWeaponEvent>();
		reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
	}

	private void OnEnable()
	{
		// Subscribe to fire weapon event.
		fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
	}

	private void OnDisable()
	{
		// Unsubscribe from fire weapon event.
		fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
	}

	/// <summary>
	/// Handle fire weapon event.
	/// </summary>
	private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
	{
		WeaponFire(fireWeaponEventArgs);
	}

	/// <summary>
	/// Fire weapon.
	/// </summary>
	private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
	{
		// Weapon fire.
		if (fireWeaponEventArgs.fire)
		{
			// Test if weapon is ready to fire.
			if (IsWeaponReadyToFire())
			{
				Shoot();
			}
		}
	}

	/// <summary>
	/// Returns true if the weapon is ready to fire, else returns false.
	/// </summary>
	private bool IsWeaponReadyToFire()
	{
		// if there is no ammo and weapon doesn't have infinite ammo then return false.
		if (activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
			return false;

		// if the weapon is reloading then return false.
		if (activeWeapon.GetCurrentWeapon().isWeaponReloading)
			return false;

		// if no ammo in the clip and the weapon doesn't have infinite clip capacity then return false.
		if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
		{
			// trigger a reload weapon event.
			reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);

			return false;
		}

		// weapon is ready to fire - return true
		return true;
	}

	public void Shoot()
	{
		
	}
}
