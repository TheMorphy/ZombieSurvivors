using UnityEngine;

public class AirdropController : Collectable
{
    private Collider collider;
	private AirdropDetails airdrop;

	private void Awake()
	{
		collider = GetComponent<Collider>();
		collider.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			Collect(other.transform, 1f);
		}
	}

	public void InitializeAirdrop(AirdropDetails airdrop)
	{
		this.airdrop = airdrop;
	}

	public void EnableCollider()
	{
		collider.enabled = true;
	}

	protected override void ActionAfterCollected()
	{
		Destroy(gameObject);
	}
}
