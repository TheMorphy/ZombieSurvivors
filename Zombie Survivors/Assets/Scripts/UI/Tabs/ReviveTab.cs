using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReviveTab : Tab, IPointerClickHandler
{
	[SerializeField] private AdsInitializer adsInitializer;

	[Space]
	[SerializeField] private int reviveCost = 5;
	[SerializeField] private Button useGems;
	[SerializeField] private TextMeshProUGUI gemsAmmountText;

	public override void Initialize(object[] args = null)
	{
		adsInitializer.OnAdClosed += AdsInitializer_OnAdClosed;

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
	}

	private void AdsInitializer_OnAdClosed(bool obj)
	{
		CloseWindow();
	}

	private void UseGemsToRevive(int currentGems)
	{
		currentGems -= reviveCost;
		PlayerPrefs.SetInt(Settings.GEMS, currentGems);
		CloseWindow();
	}

	private void CloseWindow()
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
