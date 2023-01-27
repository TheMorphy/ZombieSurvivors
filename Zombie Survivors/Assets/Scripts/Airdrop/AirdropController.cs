using UnityEngine;

public class AirdropController : MonoBehaviour
{
    private Collider collider;
    private AirdropType airdropType;

	private void Awake()
	{
		collider.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{

		}
	}

	public void EnableCollider()
	{
		collider.enabled = true;
	}
}
