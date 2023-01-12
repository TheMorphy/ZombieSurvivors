using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
	private ActiveWeapon activeWeapon;
	//private FireWeaponEvent fireWeaponEvent;
	private WeaponFiredEvent weaponFiredEvent;
	private ReloadWeaponEvent reloadWeaponEvent;

	private float timeSinceFired = 0;

	private float burstTimer;
	private int burstCounter;
	bool IsFiring = false;

	private void Awake()
	{
		// Load components.
		activeWeapon = GetComponent<ActiveWeapon>();
		//fireWeaponEvent = GetComponent<FireWeaponEvent>();
		reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
		weaponFiredEvent = GetComponent<WeaponFiredEvent>();
	}

	/// <summary>
	/// Handle fire weapon event.
	/// </summary>
	public void FireWeapn()
	{
		timeSinceFired += Time.deltaTime;

		if (IsWeaponReadyToFire())
		{
			Shoot();
		}
	}

	/// <summary>
	/// Returns true if the weapon is ready to fire, else returns false.
	/// </summary>
	private bool IsWeaponReadyToFire()
	{
		if (IsFiring == true)
			return false;

		if(activeWeapon.GetCurrentWeapon().weaponDetails.fireRate - timeSinceFired > 0)
			return false;

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

		timeSinceFired = 0;

		// weapon is ready to fire - return true
		return true;
	}

	public void Shoot()
	{
		IsFiring = true;

		int ammoPerShot = activeWeapon.GetCurrentAmmo().ammoPerShot;
		float spreadAngle = activeWeapon.GetCurrentAmmo().ammoSpread;

		// Only Spread Shot is enabled
		if (activeWeapon.GetCurrentWeapon().weaponDetails.burstFire == false && activeWeapon.GetCurrentWeapon().weaponDetails.spreadShot == true)
		{
			SpreadShot(ammoPerShot, spreadAngle);
		}
		// Only Burst Fire is enabled
		else if (activeWeapon.GetCurrentWeapon().weaponDetails.burstFire == true && activeWeapon.GetCurrentWeapon().weaponDetails.spreadShot == false)
		{
			StartCoroutine(BurstFire(ammoPerShot, spreadAngle));
		}
		// Both enabled (for the memes)
		else if (activeWeapon.GetCurrentWeapon().weaponDetails.burstFire == true && activeWeapon.GetCurrentWeapon().weaponDetails.spreadShot == true)
		{
			StartCoroutine(BurstSpreadFire(ammoPerShot, spreadAngle));
		}
		else
		{
			SingleShot(spreadAngle);
		}

		// Reduce ammo clip count if not infinite clip capacity
		if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
		{
			activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo -= ammoPerShot;
			activeWeapon.GetCurrentWeapon().weaponRemainingAmmo -= ammoPerShot;
		}

		// Call weapon fired event
		weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());

		IsFiring = false;

		//// Display weapon shoot effect
		//WeaponShootEffect(aimAngle);

		//// Weapon fired sound effect
		//WeaponSoundEffect();
	}

	private void SingleShot(float spreadAngle)
	{
		float angle = Random.Range(-spreadAngle, spreadAngle);
		Quaternion rot = Quaternion.Euler(0, angle, 0);
		Vector3 direction = rot * activeWeapon.GetShootFirePointTransform().forward;

		Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", activeWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
		ammo.InitialiseAmmo(activeWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);
	}

	private void SpreadShot(int ammoPerShot, float spreadAngle)
	{
		float stepSize = spreadAngle / (ammoPerShot - 1);

		// Loop through the number of bullets to shoot
		for (int i = 0; i < ammoPerShot; i++)
		{
			float angle = -spreadAngle / 2 + i * stepSize;
			Quaternion rotation = Quaternion.Euler(0, angle, 0);
			Vector3 direction = rotation * activeWeapon.GetShootFirePointTransform().forward;

			Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", activeWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
			ammo.InitialiseAmmo(activeWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);
		}
	}

	private IEnumerator BurstFire(int ammoPerShot, float spreadAngle)
	{
		// Decrement the burst timer
		burstTimer -= Time.deltaTime;

		// If the timer is less than zero...
		if (burstTimer < 0f)
		{
			burstTimer = activeWeapon.GetCurrentWeapon().weaponDetails.burstInterval;

			while (burstCounter < ammoPerShot)
			{
				float angle = Random.Range(-spreadAngle, spreadAngle);
				Quaternion rot = Quaternion.Euler(0, angle, 0);
				Vector3 direction = rot * activeWeapon.GetShootFirePointTransform().forward;

				// ... fire a bullet
				Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", activeWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
				ammo.InitialiseAmmo(activeWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);

				// Increment the burst counter
				burstCounter++;

				yield return new WaitForSeconds(burstTimer);
			}

			burstCounter = 0;
		}
	}

	private IEnumerator BurstSpreadFire(int ammoPerShot, float spreadAngle)
	{
		// Decrement the burst timer
		burstTimer -= Time.deltaTime;

		// If the timer is less than zero...
		if (burstTimer < 0f)
		{
			burstTimer = activeWeapon.GetCurrentWeapon().weaponDetails.burstInterval;

			while (burstCounter < ammoPerShot)
			{
				SpreadShot(ammoPerShot, spreadAngle);

				// Increment the burst counter
				burstCounter++;

				yield return new WaitForSeconds(burstTimer);
			}

			burstCounter = 0;
		}
	}
}
