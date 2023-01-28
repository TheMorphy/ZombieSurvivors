using UnityEngine;

[DisallowMultipleComponent]
public class Limb : MonoBehaviour
{
	[SerializeField] private GameObject limbPrefab;
	private Enemy enemy;

	private void Awake()
	{
		enemy = transform.root.GetComponentInParent<Enemy>();
	}

	public void GetHit(int damageAmmount)
    {
        enemy.health.TakeDamage(damageAmmount, this);
	}

	public void RemoveLimb()
	{
		GameObject limb = Instantiate(limbPrefab, transform.position, transform.rotation);

		limb.GetComponent<Rigidbody>().AddForce(transform.forward * 4f);

		transform.localScale = Vector3.zero;

		//Destroy(this);
	}
}
