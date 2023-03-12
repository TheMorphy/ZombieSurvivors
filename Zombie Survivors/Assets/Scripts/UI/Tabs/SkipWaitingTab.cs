using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipWaitingTab : Tab, IPointerClickHandler
{
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
		watchAdButton.onClick.AddListener(() => { 
			GoogleAdMobController.Instance.ShowRewardedAd();
		});

		if (args != null)
		{
			airdropSlot = (Slot<AirdropDTO>)args[0];

			Show();
			airdropDetails = airdropSlot.Details;
			gemsCost.text = airdropDetails.UnlockCost.ToString();
		}
	}

	public void UseGems()
	{
		airdropSlot.Open();
		Hide();
	}

	public void ReduceWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().Show();
		TimeTracker.Instance.DecreaseTime(airdropSlot.SlotID, airdropDetails.RemoveTime);
		Hide();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Hide();
	}
}
