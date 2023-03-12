using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReviveTab : Tab, IPointerClickHandler
{
	[Space]
	[SerializeField] private int reviveCost = 5;
	[SerializeField] private Button useGems;
	[SerializeField] private Button watchAd;
	[SerializeField] private TextMeshProUGUI gemsAmmountText;

	public override void Initialize(object[] args = null)
	{
		gemsAmmountText.text = reviveCost.ToString();

		int currentGems = PlayerPrefs.GetInt(Settings.GEMS, 10);

		if(currentGems >= reviveCost)
		{
			useGems.onClick.AddListener(() =>
			{
				UseGemsToRevive(currentGems);
			});
		}
		else
		{
			useGems.interactable = false;
		}

		watchAd.onClick.AddListener(() =>
		{
			GoogleAdMobController.Instance.ShowRewardedAd();
		});
	}

	/// <summary>
	/// After player has watched ad, revive the player. Set in inspector for google admobcontroller.
	/// </summary>
	public void RevivePlayer()
	{
		StartRevive();
	}

	private void UseGemsToRevive(int currentGems)
	{
		currentGems -= reviveCost;
		PlayerPrefs.SetInt(Settings.GEMS, currentGems);
		StartRevive();
	}

	private void StartRevive()
	{
		CanvasManager.Show<GameplayTab>();
		GameManager.Instance.RevivePlayer();
		Hide();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		CanvasManager.GetTab<GameplayTab>().ReturnToMainMenu();
	}
}
