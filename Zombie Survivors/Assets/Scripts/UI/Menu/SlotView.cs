using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
	[HideInInspector] public Slot SlotReference;

	public Sprite EmptySlotSprite;
	public TextMeshProUGUI chestTimerTxt;
	public Image chestSlotSprite;
	public TextMeshProUGUI chestTypeTxt;
	public Image coinImage;
	public TextMeshProUGUI coinsTxt;
	public Image gemImage;
	public TextMeshProUGUI gemsTxt;

	public Button ChestButton;

	private ChestState currentState;

	private void Start()
	{
		InitializeEmptyChestView();

		ChestButton.onClick.AddListener(() => {

			switch(currentState)
			{
				case ChestState.Locked:
					EnteringUnlockingState();
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
		chestTimerTxt.gameObject.SetActive(false);
		chestSlotSprite.sprite = EmptySlotSprite;
		chestTypeTxt.gameObject.SetActive(false);
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = false;
		currentState = ChestState.None;
	}

	public void InitialiseViewUIForLockedChest()
	{
		chestTimerTxt.gameObject.SetActive(false);
		chestSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		chestTypeTxt.gameObject.SetActive(true);
		chestTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
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
		chestTimerTxt.gameObject.SetActive(true);
		chestSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		chestTypeTxt.gameObject.SetActive(true);
		chestTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocking;
	}

	public void InitialiseViewUIForUnlockedChest()
	{
		chestTimerTxt.gameObject.SetActive(true);
		chestSlotSprite.sprite = SlotReference.AirdropDetails.AirdropSprite;
		chestTypeTxt.gameObject.SetActive(true);
		chestTypeTxt.text = SlotReference.AirdropDetails.AirdropType.ToString();
		coinImage.gameObject.SetActive(false);
		coinsTxt.gameObject.SetActive(false);
		gemImage.gameObject.SetActive(false);
		gemsTxt.gameObject.SetActive(false);
		ChestButton.enabled = true;
		currentState = ChestState.Unlocked;
	}

	public void EnteringUnlockingState()
	{
		InitialiseViewUIForUnlockingChest();
		StartCoroutine(SlotReference.StartTimer());
	}

	public void OpenInstantly()
	{
		InitializeEmptyChestView();
		ReceiveChestRewards();
		SlotReference.IsEmpty = true;
	}

	public void EnteringUnlockedState()
	{
		InitialiseViewUIForUnlockedChest();
		chestTimerTxt.text = "OPEN!";
	}

	public void SkipWaitingTime()
	{
		MainMenuViewController.Instance.GetSkipWaitingTabController().InitializeWindow(SlotReference);
	}

	public void OpenChest()
	{
		InitializeEmptyChestView();
		ReceiveChestRewards();
		SlotReference.IsEmpty = true;

		MainMenuViewController.Instance.GetRewardsController().InitializeWindow(SlotReference.AirdropDetails);
	}

	public void ReceiveChestRewards()
	{
		DeleteSlotPrefs();
	}

	public void DeleteSlotPrefs()
	{
		Utilities.DeletePrefs(new string[] {
		$"{SlotReference.transform.name}_QuitTimeDay",
		$"{SlotReference.transform.name}_QuitTimeHour",
		$"{SlotReference.transform.name}_QuitTimeMinute",
		$"{SlotReference.transform.name}_QuitTimeSecond",
		$"{SlotReference.transform.name}_SecondsLeft",
		$"{SlotReference.transform.name}",
		$"{SlotReference.SlotKey}"
		});
	}
}

public enum ChestState
{
	None,
	Unlocked,
	Locked,
	Unlocking
}
