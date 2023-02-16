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
		airdropSlotSprite.sprite = SlotReference.AirdropDetailsDTO.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetailsDTO.AirdropType.ToString();
		coinImage.gameObject.SetActive(true);
		coinsTxt.gameObject.SetActive(true);
		coinsTxt.text = SlotReference.AirdropDetailsDTO.UnlockCost.ToString();
		gemImage.gameObject.SetActive(true);
		gemsTxt.gameObject.SetActive(true);
		gemsTxt.text = SlotReference.AirdropDetailsDTO.UnlockCost.ToString();
		ChestButton.enabled = true;
		currentState = ChestState.Locked;
	}

	public void InitialiseViewUIForUnlockingChest()
	{
		unlockTimeText.gameObject.SetActive(true);
		airdropSlotSprite.sprite = SlotReference.AirdropDetailsDTO.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetailsDTO.AirdropType.ToString();
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
		airdropSlotSprite.sprite = SlotReference.AirdropDetailsDTO.AirdropSprite;
		airdropTypeTxt.gameObject.SetActive(true);
		airdropTypeTxt.text = SlotReference.AirdropDetailsDTO.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocked;
	}

	public void StartUnlockingAirdrop()
	{
		TimeTracker.Instance.SetNewStrackable(SlotReference);
		InitialiseViewUIForUnlockingChest();
	}

	public void SkipWaitingTime()
	{
		CanvasManager.GetTab<PlayTab>().GetSkipWaitingTab().InitializeWindow(SlotReference);
	}

	public void OpenChest()
	{
		StopAllCoroutines();
		InitializeEmptyChestView();

		CanvasManager.Show<ChestOpeningTab>(true, new object[] { SlotReference });
	}
}

public enum ChestState
{
	None,
	Unlocked,
	Locked,
	Unlocking
}
