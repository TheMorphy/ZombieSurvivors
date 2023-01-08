using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour
{
	#region Tooltip
	[Tooltip("Populate with child TrailRenderer component")]
	#endregion Tooltip
	[SerializeField] private TrailRenderer trailRenderer;

	private AmmoDetailsSO ammoDetails;
	private float ammoRange = 0f; // the range of each ammo
	private float ammoSpeed;
	private Vector3 fireDirectionVector;
	private bool isColliding = false;

	private void Update()
	{
		// Calculate distance vector to move ammo
		Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

		transform.position += distanceVector;

		// Disable after max range reached
		ammoRange -= distanceVector.magnitude;

		if (ammoRange < 0f)
		{
			DisableAmmo();
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Wall") DisableAmmo();

		// If already colliding with something return
		if (isColliding) return;

		// Deal Damage To Collision Object
		DealDamage(collision);

		// Show ammo hit effect
		// AmmoHitEffect();

		DisableAmmo();
	}

	/// <summary>
	/// Initialise the ammo being fired - using the ammodetails, the aimangle, weaponAngle, and
	/// weaponAimDirectionVector. If this ammo is part of a pattern the ammo movement can be
	/// overriden by setting overrideAmmoMovement to true
	/// </summary>
	public void InitialiseAmmo(AmmoDetailsSO ammoDetails, Vector3 launchDirection)
	{
		#region Ammo

		this.ammoDetails = ammoDetails;

		// Initialise isColliding
		isColliding = false;

		// Set fire direction
		fireDirectionVector = launchDirection;

		// Set ammo range
		ammoRange = ammoDetails.ammoRange;

		// Set ammo speed
		ammoSpeed = ammoDetails.ammoSpeed;

		// Activate ammo gameobject
		gameObject.SetActive(true);

		#endregion Ammo

		#region Trail

		if (ammoDetails.IsAmmoTrail)
		{
			trailRenderer.gameObject.SetActive(true);
			trailRenderer.emitting = true;
			trailRenderer.material = ammoDetails.AmmoTrailMaterial;
			trailRenderer.startWidth = ammoDetails.AmmoTrailStartWidth;
			trailRenderer.endWidth = ammoDetails.AmmoTrailEndWidth;
			trailRenderer.time = ammoDetails.AmmoTrailTime;
		}
		else
		{
			trailRenderer.emitting = false;
			trailRenderer.gameObject.SetActive(false);
		}

		#endregion Trail
	}

	private void DealDamage(Collider collision)
	{
		isColliding = true;

		if (collision.TryGetComponent(out Health health))
		{
			health.TakeDamage(ammoDetails.ammoDamage);
		}
	}
	private void OnDisable()
	{
		trailRenderer.gameObject.SetActive(false);
	}
	/// <summary>
	/// Disable the ammo - thus returning it to the object pool
	/// </summary>
	private void DisableAmmo()
	{
		gameObject.SetActive(false);
		trailRenderer.gameObject.SetActive(false);
	}

	public GameObject GetGameObject()
	{
		return gameObject;
	}
}
