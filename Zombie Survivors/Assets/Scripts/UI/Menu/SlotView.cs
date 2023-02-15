using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
	[HideInInspector] public Slot SlotReference;

	public Sprite EmptySlotSprite;
	public TextMeshProUGUI unlockTimeText;
	public Image airdropSlotSprite;
	public TextMeshProUGUI airdropTypeTxt;
	public Image coinImage;
	public TextMeshProUGUI coinsTxt;
	public Image gemImage;
	public TextMeshProUGUI gemsTxt;

	public Button ChestButton;

	private ChestState currentState;

	private void Start()
	{
		ChestButton.onClick.AddListener(() => {

			switch(currentState)
			{
				case ChestState.Locked:
					StartUnlockingAirdrop();
					break;
				case ChestState.Unlocking:
					SkipWaitingTime();
					break;
				case ChestState.Unlocked:
					OpenChest();
					break;
			}
		});
	}

	public void InitializeEmptyChestView()
	{
		unlockTimeText.gameObject.SetActive(false);
		airdropSlotSprite.sprite = EmptySlotSprite;
		airdropTypeTxt.gameObject.SetActive(false);
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = false;
		currentState = ChestState.None;
	}

	public void InitialiseViewUIForLockedChest()
	{
		unlockTimeText.gameObject.SetActive(false);
		airdropSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
		coinImage.gameObject.SetActive(true);
		coinsTxt.gameObject.SetActive(true);
		coinsTxt.text = SlotReference.AirdropDetails.UnlockCost.ToString();
		gemImage.gameObject.SetActive(true);
		gemsTxt.gameObject.SetActive(true);
		gemsTxt.text = SlotReference.AirdropDetails.UnlockCost.ToString();
		ChestButton.enabled = true;
		currentState = ChestState.Locked;
	}

	public void InitialiseViewUIForUnlockingChest()
	{
		unlockTimeText.gameObject.SetActive(true);
		airdropSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocking;
	}

	public void InitialiseViewUIForUnlockedChest()
	{
		unlockTimeText.gameObject.SetActive(true);
		unlockTimeText.text = "OPEN!";
		airdropSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocked;
	}

	public void StartUnlockingAirdrop()
	{
		if (SlotReference.TimerStarted == false)
			TimeTracker.Instance.StartTrackingTime(SlotReference);

		InitialiseViewUIForUnlockingChest();

		StartCoroutine(DisplayRemainingTime());
	}

	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().InitializeWindow(SlotReference);
	}

	public void OpenChest()
	{
		StopCoroutine(DisplayRemainingTime());
		InitializeEmptyChestView();

		CanvasManager.Show<ChestOpeningTab>(true, new object[] { SlotReference });
	}

	public IEnumerator DisplayRemainingTime()
	{
		print("DisplayRemainingTime");
		WaitForSeconds wait = new WaitForSeconds(1f);

		while(SlotReference.UnlockTimer > 0)
		{
			int days = (int)(SlotReference.UnlockTimer / 86400) % 365;
			int hours = (int)(SlotReference.UnlockTimer / 3600) % 24;
			int minutes = (int)(SlotReference.UnlockTimer / 60) % 60;
			int seconds = (int)(SlotReference.UnlockTimer % 60);

			unlockTimeText.text = "";

			if (days > 0) { unlockTimeText.text += days + "d "; }
			if (hours > 0) { unlockTimeText.text += hours + "h "; }
			if (minutes > 0) { unlockTimeText.text += minutes + "m "; }
			if (seconds > 0) { unlockTimeText.text += seconds + "s "; }

			SlotReference.UnlockTimer -= 1;
			yield return wait;
		}
		InitialiseViewUIForUnlockedChest();
	}
}

public enum ChestState
{
	None,
	Unlocked,
	Locked,
	Unlocking
}
