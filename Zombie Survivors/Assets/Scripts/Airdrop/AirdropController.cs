using UnityEngine;

public class AirdropController : Collectable
{
    private Collider collider;
	private AirdropDetails airdrop;
	private Player player;

	[SerializeField] private GameObject crashmark;
	[SerializeField] private float absorbTime = 1.5f;

	Vector3 startPos;
	private void Awake()
	{
		collider = GetComponent<Collider>();
		collider.enabled = false;
	}
	private void Start()
	{
		startPos = transform.position;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			player = other.GetComponent<Player>();
			DisableCrashmark();
			StartCoroutine(Collect(other.transform, absorbTime));
		}
	}

	public void InitializeAirdrop(AirdropDetails airdrop)
	{
		this.airdrop = Instantiate(airdrop);
	}

	public void EnableCollider()
	{
		collider.enabled = true;
	}

	private void DisableCrashmark()
	{
		crashmark.SetActive(false);
	}

	protected override void OnCollected()
	{
		StaticEvents.CallCollectedEvent(startPos);

		CanvasManager.Instance.GetActiveCanvas().HideAirdropAlert();

		Destroy(gameObject);
	}
}
