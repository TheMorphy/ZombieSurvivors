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

	private const float contactDamageCollisionResetDelay = 1f;

	private int contactDamageAmount;

	private Enemy enemy;

	private void Awake()
	{
		enemy = transform.root.GetComponent<Enemy>();
	}
	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (isColliding) return;

	//	ContactDamage(collision);
	//}

	//private void OnCollisionExit(Collision collision)
	//{
	//	if (isColliding) return;

	//	ContactDamage(collision);
	//}

	private void OnTriggerEnter(Collider collider)
	{
		if (isColliding) return;

		ContactDamage(collider);
	}

	//private void OnTriggerStay(Collider collider)
	//{
	//	// If already colliding with something return
	//	if (isColliding) return;

	//	ContactDamage(collider);
	//}

	private void ContactDamage(Collider collision)
	{
		// if the collision object isn't in the specified layer then return (use bitwise comparison)
		int collisionObjectLayerMask = (1 << collision.gameObject.layer);

		if ((layerMask.value & collisionObjectLayerMask) == 0)
			return;

		print("Player Hit");

		// Check to see if the colliding object should take contact damage
		ReceiveContactDamage receiveContactDamage = collision.gameObject.GetComponent<ReceiveContactDamage>();

		if (receiveContactDamage != null)
		{
			isColliding = true;

			// Reset the contact collision after set time
			Invoke("ResetContactCollision", contactDamageCollisionResetDelay);

			receiveContactDamage.TakeContactDamage(contactDamageAmount);
		}
		enemy.enemyController.DisableHitboxes();
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