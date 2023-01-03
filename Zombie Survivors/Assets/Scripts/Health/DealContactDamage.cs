using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
	#region Header DEAL DAMAGE
	[Space(10)]
	[Header("DEAL DAMAGE")]
	#endregion
	
	#region Tooltip
	[Tooltip("Specify what layers objects should be on to receive contact damage")]
	#endregion
	[SerializeField] private LayerMask layerMask;
	private bool isColliding = false;

	private const float contactDamageCollisionResetDelay = 0.5f;

	private int contactDamageAmount;

	private void OnCollisionEnter(Collision collision)
	{
		if (isColliding) return;

		ContactDamage(collision.collider);
	}
	private void OnCollisionStay(Collision collision)
	{
		// If already colliding with something return
		if (isColliding) return;

		ContactDamage(collision.collider);
	}

	private void ContactDamage(Collider collision)
	{
		// if the collision object isn't in the specified layer then return (use bitwise comparison)
		int collisionObjectLayerMask = (1 << collision.gameObject.layer);

		if ((layerMask.value & collisionObjectLayerMask) == 0)
			return;

		// Check to see if the colliding object should take contact damage
		ReceiveContactDamage receiveContactDamage = collision.gameObject.GetComponent<ReceiveContactDamage>();

		if (receiveContactDamage != null)
		{
			isColliding = true;

			// Reset the contact collision after set time
			Invoke("ResetContactCollision", contactDamageCollisionResetDelay);

			receiveContactDamage.TakeContactDamage(contactDamageAmount);

		}

	}

	public void SetContactDamage(int contactDamage)
	{
		contactDamageAmount = contactDamage;
	}

	/// <summary>
	/// Reset the isColliding bool
	/// </summary>
	private void ResetContactCollision()
	{
		isColliding = false;
	}
}