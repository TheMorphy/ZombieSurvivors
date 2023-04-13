using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
	private Comrade comrade;

	private float timeSinceFired = 0;
	private float burstTimer;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();
	}

	public void Update()
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
		if (comrade.ActiveWeapon.GetCurrentWeapon().WeaponDisabled)
			return false;

		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.FireRate - timeSinceFired > 0)
			return false;

		// if the weapon is reloading then return false.
		if (comrade.ActiveWeapon.GetCurrentWeapon().isWeaponReloading)
			return false;

		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.InfiniteAmmo)
			return false;

		// if there is no ammo int he clip and weapon doesn't have infinite ammo then return false.
		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponClipRemainingAmmo < comrade.ActiveWeapon.GetCurrentAmmo().ammoPerShot && !comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.InfiniteMagazineCapacity)
		{
			comrade.ReloadWeaponEvent.CallReloadWeaponEvent(comrade.ActiveWeapon.GetCurrentWeapon());
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
		if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.BurstFire == false && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.SpreadShot == true)
		{
			SpreadShot(ammoPerShot, spreadAngle);
		}
		// Only Burst Fire is enabled
		else if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.BurstFire == true && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.SpreadShot == false)
		{
			StartCoroutine(BurstFire(ammoPerShot, spreadAngle));
		}
		// Both enabled (for the memes)
		else if (comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.BurstFire == true && comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.SpreadShot == true)
		{
			StartCoroutine(BurstSpreadFire(ammoPerShot, spreadAngle));
		}
		else
		{
			SingleShot(spreadAngle);
		}

		// Reduce ammo clip count if not infinite clip capacity
		if (!comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.InfiniteMagazineCapacity)
		{
			comrade.ActiveWeapon.GetCurrentWeapon().weaponClipRemainingAmmo -= ammoPerShot;
			comrade.ActiveWeapon.GetCurrentWeapon().weaponRemainingAmmo -= ammoPerShot;
		}

		// Call weapon fired event
		comrade.WeaponFiredEvent.CallWeaponFiredEvent(comrade.ActiveWeapon.GetCurrentWeapon());
	}

	private void SingleShot(float spreadAngle)
	{
		Vector3 direction = GetRandomDirection(spreadAngle);
		FireAmmo(direction);
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
			FireAmmo(direction);
		}
	}

	private IEnumerator BurstFire(int ammoPerShot, float spreadAngle)
	{
		// Decrement the burst timer
		burstTimer -= Time.deltaTime;

		// If the timer is less than zero...
		if (burstTimer < 0f)
		{
			burstTimer = comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.BurstInterval;

			for (int i = 0; i < ammoPerShot; i++)
			{
				Vector3 direction = GetRandomDirection(spreadAngle);
				FireAmmo(direction);
				yield return new WaitForSeconds(burstTimer);
			}
		}
	}

	private IEnumerator BurstSpreadFire(int ammoPerShot, float spreadAngle)
	{
		// Decrement the burst timer
		burstTimer -= Time.deltaTime;

		// If the timer is less than zero...
		if (burstTimer < 0f)
		{
			burstTimer = comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.BurstInterval;

			for (int i = 0; i < ammoPerShot; i++)
			{
				SpreadShot(ammoPerShot, spreadAngle);
				yield return new WaitForSeconds(burstTimer);
			}
		}
	}

	private Vector3 GetRandomDirection(float spreadAngle)
	{
		float angle = Random.Range(-spreadAngle, spreadAngle);
		Quaternion rot = Quaternion.Euler(angle, angle, 0);
		Vector3 direction = rot * comrade.ActiveWeapon.GetShootFirePointTransform().forward;
		return direction;
	}

	private void FireAmmo(Vector3 direction)
	{
		Ammo ammo = PoolManager.Instance.SpawnFromPool("Ammo", comrade.ActiveWeapon.GetShootPosition(), Quaternion.identity).GetComponent<Ammo>();
		ammo.InitialiseAmmo(comrade.ActiveWeapon.GetCurrentWeapon().weaponDetails.AmmoDetails, direction);
	}


}
