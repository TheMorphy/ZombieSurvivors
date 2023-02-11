using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkipWaitingTab : MonoBehaviour
{
	[SerializeField] private AdsInitializer adsInitializer;

	[Space]
    [Header("Gems")]
    [SerializeField] private TextMeshProUGUI gemsCost;
    [SerializeField] private Button useGemsButton;

	[Space]
	[Header("Ads")]
	[SerializeField] private TextMeshProUGUI skipTimeWithAdsText;
	[SerializeField] private Button watchAdButton;

	private AirdropDetails airdropDetails;
	private Slot slot;

	private void Start()
	{
		adsInitializer.InitializeAd();
	}

	private void OnEnable()
	{
		adsInitializer.OnAdClosed += AdsInitializer_OnAdClosed;
	}

	private void OnDisable()
	{
		adsInitializer.OnAdClosed -= AdsInitializer_OnAdClosed;
	}

	private void AdsInitializer_OnAdClosed(bool giveReward)
	{
		if (giveReward)
		{
			MainMenuViewController.Instance.GetSlotsController().Show();
			slot.RemoveTime(1800);
			gameObject.SetActive(false);
			slot.GetSlotView().ChestButton.enabled = true;
		}
	}

	public void InitializeWindow(Slot slot)
	{
		this.slot = slot;

		slot.GetSlotView().ChestButton.enabled = false;

		gameObject.SetActive(true);

		airdropDetails = slot.AirdropDetails;

		gemsCost.text = airdropDetails.UnlockCost.ToString();

		useGemsButton.onClick.AddListener(() => {
			
			slot.RemoveTime(Mathf.Infinity);
			slot.GetSlotView().EnteringUnlockedState();
			gameObject.SetActive(false);
		});
	}
}
