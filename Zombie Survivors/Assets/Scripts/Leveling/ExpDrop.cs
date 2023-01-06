using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[DisallowMultipleComponent]
public class ExpDrop : MonoBehaviour
{
	[SerializeField] private float absorbTime = 1.5f;

	private Rigidbody rb;

	private int expAmmount;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void Start()
	{
		transform.position += Vector3.up * 2f;

		rb.velocity = new Vector2(Random.onUnitSphere.x, Random.onUnitSphere.z) * 3f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			StartCoroutine(GoTowardsPlayer(other.transform));
		}
	}

	private IEnumerator GoTowardsPlayer(Transform playerTransform)
	{
		float elapsedTime = 0;

		while (elapsedTime < absorbTime)
		{
			transform.position = Vector3.Lerp(transform.position, playerTransform.position, (elapsedTime / absorbTime));
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (elapsedTime / absorbTime));
			elapsedTime += Time.deltaTime;

			if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
			{
				GameManager.Instance.GetLevelSystem().AddExperience(expAmmount);
				Destroy(gameObject);
			}

			yield return null;
		}
	}

	public void SetExpValue(int expAmmount)
	{
		this.expAmmount = expAmmount;
	}

	public int CollectExp()
	{
		return expAmmount;
	}
}
