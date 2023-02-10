using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Airdrop : Collectable
{
    private Collider collider;
	private AirdropDetails airdrop;

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
			DisableCrashmark();
			StartCoroutine(Collect(other.transform, absorbTime));
		}
	}

	public void InitializeAirdrop(AirdropDetails airdrop)
	{
		this.airdrop = Instantiate(airdrop);
	}

	public void OnPackageLanded()
	{
		CanvasManager.Instance.GetActiveCanvas().HideAirdropAlert();

		collider.enabled = true;
	}

	private void DisableCrashmark()
	{
		crashmark.SetActive(false);
	}

	protected override void OnCollected()
	{
		bool airdropSaved = false;

		for (int i = 0; i < 4; i++)
		{
			if (PlayerPrefs.HasKey($"{airdrop.AirdropType + "_" + i}") == false)
			{
				PlayerPrefs.SetString(airdrop.AirdropType + "_" + i, airdrop.AirdropType.ToString());
				airdropSaved = true;
				break;
			}
		}

		if(airdropSaved == false)
		{
			// TODO: Add money instead
		}

		StaticEvents.CallCollectedEvent(startPos);

		Destroy(gameObject);
	}


}
