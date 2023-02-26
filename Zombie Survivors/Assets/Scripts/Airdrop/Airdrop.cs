using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Airdrop : Collectable
{
    private Collider collider;
	private AirdropDetails airdropDetails;

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
			SaveAirdrop();
			DisableCrashmark();
			StartCoroutine(Collect(other.transform, absorbTime));
		}
	}

	protected override void OnCollected()
	{
		StaticEvents.CallCollectedEvent(startPos);
		Destroy(gameObject);
	}

	public void InitializeAirdrop(AirdropDetails airdrop)
	{
		airdropDetails = Instantiate(airdrop);
	}

	public void OnPackageLanded()
	{
		CanvasManager.GetTab<GameplayTab>().HideAirdropAlert();

		collider.enabled = true;
	}

	private void DisableCrashmark()
	{
		crashmark.SetActive(false);
	}

	private void SaveAirdrop()
	{
		var savedAirdrops = SaveManager.ReadFromJSON<AirdropDTO>(Settings.AIRDROPS);

		if (savedAirdrops.Count < CollectedAirdropsController.MAX_SLOT_COUNT)
		{
			AudioManager.Instance.PlaySFX(SoundTitle.Airdrop_Pickup);

			AirdropDTO collectedAirrop = new AirdropDTO()
			{
				Code = Guid.NewGuid().ToString(),
				AirdropSprite = airdropDetails.AirdropSprite,
				AirdropPackage = airdropDetails.AirdropPackage,
				AirdropType = airdropDetails.AirdropType,
				CardAmmount = airdropDetails.CardAmmount,
				MaxGemsAmmount = airdropDetails.MaxGemsAmmount,
				MaxGoldAmmount = airdropDetails.MaxGoldAmmount,
				MinGemsAmmount = airdropDetails.MinGemsAmmount,
				MinGoldAmmount = airdropDetails.MinGoldAmmount,
				RemoveTime = airdropDetails.RemoveTime,
				UnlockCost = airdropDetails.UnlockCost,
				UnlockDuration = airdropDetails.UnlockDuration,
			};

			SaveManager.SaveToJSON(collectedAirrop, Settings.AIRDROPS);
		}
		else
		{
			AudioManager.Instance.PlaySFX(SoundTitle.Money_Pickup);

			// TODO: Gib money
			int currentAmmount = PlayerPrefs.GetInt(Settings.GOLD_AMMOUNT, 100);
			currentAmmount += UnityEngine.Random.Range(50, 300);

			PlayerPrefs.SetInt(Settings.GOLD_AMMOUNT, currentAmmount);
		}
	}
}