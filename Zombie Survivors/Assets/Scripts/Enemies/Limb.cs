using UnityEngine;

[DisallowMultipleComponent]
public class Limb : MonoBehaviour
{
	[SerializeField] private GameObject limbPrefab;
	[SerializeField] private GameObject bloodFX;
	private Enemy enemy;

	private GameObject limbObject;

	[HideInInspector] public bool limbShot = false;

	private void Awake()
	{
		enemy = transform.root.GetComponent<Enemy>();
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

	public void GetHit(int damageAmmount)
    {
		enemy.health.TakeDamage(damageAmmount);
	}

	public void RemoveLimb(Vector3 forceDirection, float force)
	{
		limbShot = true;

		transform.localScale = Vector3.zero;

		limbObject = Instantiate(limbPrefab, transform.position, transform.rotation);
		limbObject.GetComponent<Rigidbody>().AddForce(forceDirection * force * 10f);

		Instantiate(bloodFX, limbObject.transform.position, limbObject.transform.rotation, transform);
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
