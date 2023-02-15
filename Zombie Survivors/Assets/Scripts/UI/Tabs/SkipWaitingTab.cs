using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipWaitingTab : MonoBehaviour, IPointerClickHandler
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
			CanvasManager.GetTab<PlayTab>().Show();
			slot.RemoveTime(airdropDetails.RemoveTime);
			slot.GetSlotView().ChestButton.enabled = true;
			Hide();
		}
	}

	public void InitializeWindow(Slot slot)
	{
		Show();

		this.slot = slot;
		airdropDetails = slot.AirdropDetails;
		gemsCost.text = airdropDetails.UnlockCost.ToString();

		useGemsButton.onClick.AddListener(() => {
			slot.GetSlotView().OpenChest();
			Hide();
		});
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Hide();
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}
}
