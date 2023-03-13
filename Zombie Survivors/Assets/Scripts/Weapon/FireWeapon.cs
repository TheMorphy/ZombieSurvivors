using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
	private Comrade comrade;

	private float timeSinceFired = 0;

	private float burstTimer;
	private int burstCounter;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();
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
		if(comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.fireRate - timeSinceFired > 0)
			return false;

		// if the weapon is reloading then return false.
		if (comrade.ActiveWeapon.GetCurrentWeapon().isWeaponReloading)
			return false;

		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
			return false;

		// if there is no ammo int he clip and weapon doesn't have infinite ammo then return false.
		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponClipRemainingAmmo < comrade.ActiveWeapon.GetCurrentAmmo().ammoPerShot && !comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
		{
			comrade.ReloadWeaponEvent.CallReloadWeaponEvent(comrade.ActiveWeapon.GetCurrentWeapon(), 0);
			return false;
		}

		timeSinceFired = 0;

		// weapon is ready to fire - return true
		return true;
	}

	public void Shoot()
	{
		int ammoPerShot = comrade.ActiveWeapon.GetCurrentAmmo().ammoPerShot;
		float spreadAngle = comrade.ActiveWeapon.GetCurrentAmmo().ammoSpread;

		// Only Spread Shot is enabled
		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.burstFire == false && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.spreadShot == true)
		{
			SpreadShot(ammoPerShot, spreadAngle);
		}
		// Only Burst Fire is enabled
		else if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.burstFire == true && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.spreadShot == false)
		{
			StartCoroutine(BurstFire(ammoPerShot, spreadAngle));
		}
		// Both enabled (for the memes)
		else if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.burstFire == true && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.spreadShot == true)
		{
			StartCoroutine(BurstSpreadFire(ammoPerShot, spreadAngle));
		}
		else
		{
			SingleShot(spreadAngle);
		}

		// Reduce ammo clip count if not infinite clip capacity
		if (!comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
		{
			comrade.ActiveWeapon.GetCurrentWeapon().weaponClipRemainingAmmo -= ammoPerShot;
			comrade.ActiveWeapon.GetCurrentWeapon().weaponRemainingAmmo -= ammoPerShot;
		}

		// Call weapon fired event
		comrade.WeaponFiredEvent.CallWeaponFiredEvent(comrade.ActiveWeapon.GetCurrentWeapon());
	}

	private void SingleShot(float spreadAngle)
	{
		float angle = Random.Range(-spreadAngle, spreadAngle);
		Quaternion rot = Quaternion.Euler(angle, angle, 0);
		Vector3 direction = rot * comrade.ActiveWeapon.GetShootFirePointTransform().forward;

		Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", comrade.ActiveWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
		ammo.InitialiseAmmo(comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);
	}

	private void SpreadShot(int ammoPerShot, float spreadAngle)
	{
		float stepSize = spreadAngle / (ammoPerShot - 1);

		// Loop through the number of bullets to shoot
		for (int i = 0; i < ammoPerShot; i++)
		{
			float angle = -spreadAngle / 2 + i * stepSize;
			Quaternion rotation = Quaternion.Euler(angle, angle, 0);
			Vector3 direction = rotation * comrade.ActiveWeapon.GetShootFirePointTransform().forward;

			Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", comrade.ActiveWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
			ammo.InitialiseAmmo(comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);
		}
	}

	private IEnumerator BurstFire(int ammoPerShot, float spreadAngle)
	{
		// Decrement the burst timer
		burstTimer -= Time.deltaTime;

		// If the timer is less than zero...
		if (burstTimer < 0f)
		{
			burstTimer = comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.burstInterval;

			while (burstCounter < ammoPerShot)
			{
				float angle = Random.Range(-spreadAngle, spreadAngle);
				Quaternion rot = Quaternion.Euler(angle, angle, 0);
				Vector3 direction = rot * comrade.ActiveWeapon.GetShootFirePointTransform().forward;

				// ... fire a bullet
				Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", comrade.ActiveWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
				ammo.InitialiseAmmo(comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);

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
			burstTimer = comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.burstInterval;

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
