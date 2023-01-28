using UnityEngine;

[DisallowMultipleComponent]
public class Limb : MonoBehaviour
{
	[SerializeField] private GameObject limbPrefab;
	private Limb[] childLimbs;

	private Enemy enemy;
	CharacterJoint joint;
	Rigidbody rb;

	private void Awake()
	{
		//childLimbs = GetComponentsInChildren<Limb>();
		enemy = transform.root.GetComponentInParent<Enemy>();
		joint = GetComponent<CharacterJoint>();
		rb = GetComponent<Rigidbody>();
	}

	public void GetHit(int damageAmmount)
    {
        enemy.health.TakeDamage(damageAmmount, this);
	}

	public void RemoveLimb()
	{
		//for (int i = 0; i < childLimbs.Length; i++)
		//{
		//	if (childLimbs[i] != null)
		//	{
		//		childLimbs[i].transform.localScale = Vector3.zero;
		//		Destroy(childLimbs[i]);
		//	}
		//}

		Instantiate(limbPrefab, transform.position, transform.rotation);

		//limb.GetComponent<Rigidbody>().AddForce(transform.forward * 10f);

		transform.localScale = Vector3.zero;

		//Destroy(this);
	}
}
