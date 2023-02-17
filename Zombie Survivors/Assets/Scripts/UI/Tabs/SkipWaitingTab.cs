using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipWaitingTab : Tab, IPointerClickHandler
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

	private AirdropDTO airdropDetails;
	private Slot<AirdropDTO> airdropSlot;

	public override void Initialize(object[] args)
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
			CanvasManager.GetTab<PlayTab>().Show();
			TimeTracker.Instance.DecreaseTime(airdropSlot.SlotID, airdropDetails.RemoveTime);
			Hide();
		}
	}

	public void InitializeWindow(Slot<AirdropDTO> airdropSlot)
	{
		Show();

		this.airdropSlot = airdropSlot;
		airdropDetails = airdropSlot.Details;
		gemsCost.text = airdropDetails.UnlockCost.ToString();

		useGemsButton.onClick.AddListener(() =>
		{
			airdropSlot.Open();
			Hide();
		});
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Hide();
	}
}
