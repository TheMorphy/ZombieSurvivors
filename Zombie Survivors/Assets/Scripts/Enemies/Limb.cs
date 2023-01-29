using UnityEngine;

[DisallowMultipleComponent]
public class Limb : MonoBehaviour
{
	[SerializeField] private GameObject limbPrefab;
	private Enemy enemy;

	GameObject limbObject;
	Vector3 forceDirection;
	float force;

	private void Awake()
	{
		enemy = transform.root.GetComponentInParent<Enemy>();
	}

	private void OnEnable()
	{
		enemy.destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
	}

	private void OnDisable()
	{
		enemy.destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
	}

	private void DestroyedEvent_OnDestroyed(DestroyedEvent arg1, DestroyedEventArgs destroyedEventArgs)
	{
		if(destroyedEventArgs.playerDied == false)
		{
			DestroyLimb();
		}
	}

	public void GetHit(int damageAmmount, Vector3 forceDirection, float force)
    {
		this.forceDirection = forceDirection;
		this.force = force;

		enemy.health.TakeDamage(damageAmmount, this);
	}

	public void RemoveLimb(out float forceToAdd, out Vector3 forceDirection)
	{
		forceToAdd = force;
		forceDirection = this.forceDirection;

		limbObject = Instantiate(limbPrefab, transform.position, transform.rotation);

		limbObject.GetComponent<Rigidbody>().AddForce(forceDirection * forceToAdd * 15f);

		transform.localScale = Vector3.zero;
	}

	public void DestroyLimb()
	{
		Destroy(limbObject);
	}

	//private IEnumerator Destroy()
	//{
	//	float counter = 0;
	//	float duration = 1f;
	//	yield return new WaitForSeconds(2f);

	//	while (counter < duration)
	//	{
	//		counter += Time.deltaTime;
	//		limbObject.transform.localScale = Vector3.Lerp(limbObject.transform.localScale, Vector3.zero, counter / duration);
	//		yield return null;
	//	}
	//}
}
