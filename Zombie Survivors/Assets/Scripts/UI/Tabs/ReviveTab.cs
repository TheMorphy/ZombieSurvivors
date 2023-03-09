using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveTab : Tab
{
	[SerializeField] private AdsInitializer adsInitializer;

	[Space(10)]
	[Header("GENERAL FIELDS")]
	[SerializeField] private Button exit;

	[Space]
	[Header("USE FIELDS")]
	[SerializeField] private int reviveGemsCost = 5;
	[SerializeField] private Button useGems;
	[SerializeField] private TextMeshProUGUI gemsAmmountText;

	[Space(10)]
	[Header("TIMER PROPERTIES")]
	[SerializeField] private TextMeshProUGUI secondsLeftText;
	[SerializeField] private int waitTime = 5;
	[SerializeField] private int remainingTime = 0;
	[SerializeField] private Image waitImage;

	public override void Initialize(object[] args = null)
	{
		adsInitializer.OnAdClosed += AdsInitializer_OnAdClosed;

		gemsAmmountText.text = reviveGemsCost.ToString();
		remainingTime = waitTime;
		transform.localScale = Vector3.zero;

		int currentGems = PlayerPrefs.GetInt(Settings.GEMS, 10);

		if(currentGems >= reviveGemsCost)
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

		exit.onClick.AddListener(() =>
		{
			BackToMainMenu();
		});
	}

	private void AdsInitializer_OnAdClosed(bool obj)
	{
		CloseWindow();
	}

	private void UseGemsToRevive(int currentGems)
	{
		currentGems -= reviveGemsCost;
		PlayerPrefs.SetInt(Settings.GEMS, currentGems);
		CloseWindow();
	}

	public override void Show()
	{
		base.Show();
		transform.DOScale(1f, 0.6f).SetEase(Ease.OutBack).OnComplete(() => StartCoroutine(StartCountdown()));
	}

	private void CloseWindow()
	{
		StopAllCoroutines();

		transform.DOScale(0f, 0.4f).SetEase(Ease.InSine).OnComplete(() => {
			ResetWindow();
			CanvasManager.Show<GameplayTab>();
			GameManager.Instance.RevivePlayer();
			Hide();
		});
	}

	private IEnumerator StartCountdown()
	{
		float difference = waitTime;

		yield return new WaitForSeconds(0.2f); // Short pre-delay

		while (remainingTime > 0)
		{
			difference -= Time.deltaTime;
			remainingTime = Mathf.CeilToInt(difference);

			secondsLeftText.text = remainingTime.ToString();
			waitImage.fillAmount = difference / waitTime;

			yield return null;
		}
		OnTimerEnded();
	}

	private void BackToMainMenu()
	{
		transform.DOScale(0f, 0.4f).SetEase(Ease.InSine).OnComplete(() => {
			CanvasManager.GetTab<GameplayTab>().ReturnToMainMenu();
		});
	}

	private void OnTimerEnded()
	{
		BackToMainMenu();
	}

	private void ResetWindow()
	{
		waitImage.fillAmount = 1f;
		remainingTime = waitTime;
		secondsLeftText.text = remainingTime.ToString();
	}
}
