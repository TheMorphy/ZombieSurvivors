using System;
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
			DisableCrashmark();
			StartCoroutine(Collect(other.transform, absorbTime));
		}
	}

	public void InitializeAirdrop(AirdropDetails airdrop)
	{
		this.airdropDetails = Instantiate(airdrop);
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

	protected override void OnCollected()
	{
		AirdropDTO collectedAirrop = new AirdropDTO
		{
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

		if (SaveManager.GetNumSavedItems<AirdropDTO>(Settings.AIRDROPS) < Settings.AVAILABLE_AIRDROP_SLOTS_COUNT)
		{
			SaveManager.SaveToJSON(collectedAirrop, Settings.AIRDROPS);
		}
		else
		{
			// TODO: Gib money
		}

		StaticEvents.CallCollectedEvent(startPos);

		Destroy(gameObject);
	}
}

#region For Serializing Airdrop Scriptable Object To File
[Serializable]
public class AirdropDTO
{
	public AirdropType AirdropType;
	public Sprite AirdropSprite;
	public GameObject AirdropPackage;
	public int MaxGemsAmmount;
	public int MinGemsAmmount;
	public int MinGoldAmmount;
	public int MaxGoldAmmount;
	public int UnlockDuration;
	public int CardAmmount;
	public int UnlockCost;
	public int RemoveTime;
}
#endregion