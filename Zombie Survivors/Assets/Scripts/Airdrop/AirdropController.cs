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
			Destroy(gameObject);
		}
	}

	public void SetAirdropType(AirdropType airdropType)
	{
		this.airdropType = airdropType;
	}

	public void EnableCollider()
	{
		collider.enabled = true;
	}
}
