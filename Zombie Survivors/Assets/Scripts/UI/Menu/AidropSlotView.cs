using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AidropSlotView : MonoBehaviour
{
	[HideInInspector] public AidropSlot AirdropSlot;

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
					AirdropSlot.StartTracking();
					break;
				case ChestState.Unlocking:
					AirdropSlot.SkipWaitingTime();
					break;
				case ChestState.Unlocked:
					AirdropSlot.Open();
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
		airdropSlotSprite.sprite = AirdropSlot.Details.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = AirdropSlot.Details.AirdropType.ToString();
		coinImage.gameObject.SetActive(true);
		coinsTxt.gameObject.SetActive(true);
		coinsTxt.text = AirdropSlot.Details.UnlockCost.ToString();
		gemImage.gameObject.SetActive(true);
		gemsTxt.gameObject.SetActive(true);
		gemsTxt.text = AirdropSlot.Details.UnlockCost.ToString();
		ChestButton.enabled = true;
		currentState = ChestState.Locked;
	}

	public void InitialiseViewUIForUnlockingChest()
	{
		unlockTimeText.gameObject.SetActive(true);
		airdropSlotSprite.sprite = AirdropSlot.Details.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = AirdropSlot.Details.AirdropType.ToString();
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
		airdropSlotSprite.sprite = AirdropSlot.Details.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = AirdropSlot.Details.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocked;
	}
}

public enum ChestState
{
	None,
	Unlocked,
	Locked,
	Unlocking
}
