using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameplayTab : Tab
{
	[Space]
	[Header("CONTROLLERS")]
	[SerializeField] private PointerController pointerController;

	[Space]
    [Header("BUTTONS")]
    [SerializeField] private Button backToMainMenu;

    [Space]
    [Header("IMAGES")]
    [SerializeField] private Image bossHealthbar;

	[Space]
	[Header("TEXT")]
	[SerializeField] private TextMeshProUGUI timer;
	[SerializeField] private TextMeshProUGUI airdropIncomming;

	public override void Initialize(object[] args)
	{
		bossHealthbar.transform.parent.gameObject.SetActive(false);
		backToMainMenu.gameObject.SetActive(false);
		airdropIncomming.gameObject.SetActive(false);

		backToMainMenu.onClick.AddListener(() => {
			ReturnToMainMenu();
		});
	}

	private void Update()
	{
		timer.text = TimeTracker.Instance.GetFormattedGameTime();
	}

	private void OnEnable()
	{
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

		StaticEvents.OnAirdropSpawned += StaticEvents_OnAirdropSpawned;

		StaticEvents.OnCircleSpawned += StaticEvents_OnCircleSpawned;

		StaticEvents.OnCollected += StaticEvents_OnCollected;
	}

	private void OnDisable()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

		StaticEvents.OnAirdropSpawned -= StaticEvents_OnAirdropSpawned;

		StaticEvents.OnCircleSpawned -= StaticEvents_OnCircleSpawned;

		StaticEvents.OnCollected -= StaticEvents_OnCollected;
	}

	

	private void GameManager_OnGameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.BossFight:
				ShowBossHealth();
				break;
			case GameState.Evacuating:
				HideBossHealth();
				break;
			case GameState.GameWon:
				ShowExitButton();
				break;
			case GameState.GameLost:
				ShowReviveWindow();
				break;
		}
	}

	private void ShowReviveWindow()
	{
		CanvasManager.Show<ReviveTab>(false);
	}

	private void StaticEvents_OnAirdropSpawned(Vector3 airdropSpawnPosition)
	{
		DisplayAirdropAlert();

		pointerController.CreateAirdropTargetPointer(airdropSpawnPosition);
	}

	private void StaticEvents_OnCircleSpawned(CircleSpawnedEventArgs circleSpawnedEventArgs)
	{
		pointerController.CreateMultiplicationTargetPointer(circleSpawnedEventArgs.spawnPosition);
	}

	private void StaticEvents_OnCollected(CollectedEventArgs circleDespawnedEventArgs)
	{
		pointerController.targetPointers.Find(x => x.targetPosition == circleDespawnedEventArgs.collectedPosition)?.DestroySelf();
	}

	public Image GetBossHealth()
	{
		return bossHealthbar;
	}

	public void DisplayAirdropAlert()
	{
		airdropIncomming.text = "!! AIRDROP INCOMMING !!";
		airdropIncomming.gameObject.SetActive(true);
	}

	public void HideAirdropAlert()
	{
		airdropIncomming.gameObject.SetActive(false);
	}

	public void ShowBossHealth()
	{
		bossHealthbar.transform.parent.gameObject.SetActive(true);
	}

	public void HideBossHealth()
	{
		bossHealthbar.transform.parent.gameObject.SetActive(false);
	}

	public void ShowExitButton()
	{
		backToMainMenu.gameObject.SetActive(true);
	}

	public void ReturnToMainMenu()
	{
		AudioManager.Instance.PlayMusicWithFade(SoundTitle.MainMenu_Theme, 1f);
		SceneManager.LoadScene(0);
	}
}
