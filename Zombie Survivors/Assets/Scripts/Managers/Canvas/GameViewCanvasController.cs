using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameViewCanvasController : MonoBehaviour
{
	public CanvasType canvasType;
	private CanvasManager canvasManager;

	private UpgradeSystem upgradesController;
	private Pointer pointer;

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

	private void Awake()
	{
		canvasManager = GetComponentInParent<CanvasManager>();
		pointer = GetComponentInChildren<Pointer>();
		upgradesController = GetComponentInChildren<UpgradeSystem>();
	}

	private void Start()
	{
		upgradesController.SetLevelSystem(GameManager.Instance.GetLevelSystem());

		bossHealthbar.transform.parent.gameObject.SetActive(false);
		backToMainMenu.gameObject.SetActive(false);
		airdropIncomming.gameObject.SetActive(false);

		backToMainMenu.onClick.AddListener(() => {
			canvasManager.BackToMainMenu();
		});
	}

	private void Update()
	{
		DisplayTime();
	}

	private void OnEnable()
	{
		upgradesController.OnUpgradeSet += UpgradesController_OnUpgradeSet;

		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;

		StaticEvents.OnAirdropSpawned += StaticEvents_OnAirdropSpawned;

		StaticEvents.OnCircleSpawned += StaticEvents_OnCircleSpawned;

		StaticEvents.OnCollected += StaticEvents_OnCollected;
	}

	private void OnDisable()
	{
		upgradesController.OnUpgradeSet -= UpgradesController_OnUpgradeSet;

		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;

		StaticEvents.OnAirdropSpawned -= StaticEvents_OnAirdropSpawned;

		StaticEvents.OnCircleSpawned -= StaticEvents_OnCircleSpawned;

		StaticEvents.OnCollected -= StaticEvents_OnCollected;
	}

	private void GameManager_OnGameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.bossFight:
				ShowBossHealth();
				break;
			case GameState.evacuating:
				HideBossHealth();
				break;
			case GameState.gameWon:
				ShowExitButton();
				break;
		}
	}

	private void StaticEvents_OnAirdropSpawned(Vector3 airdropSpawnPosition)
	{
		DisplayAirdropAlert();

		pointer.CreateAirdropTargetPointer(airdropSpawnPosition);
	}

	private void UpgradesController_OnUpgradeSet()
	{
		upgradesController.Hide();

		GameManager.Instance.GetPlayer().playerController.EnablePlayerMovement();
	}

	private void StaticEvents_OnCircleSpawned(CircleSpawnedEventArgs circleSpawnedEventArgs)
	{
		pointer.CreateMultiplicationTargetPointer(circleSpawnedEventArgs.spawnPosition);
	}

	private void StaticEvents_OnCollected(CollectedEventArgs circleDespawnedEventArgs)
	{
		pointer.targetPointers.First(x => x.targetPosition == circleDespawnedEventArgs.collectedPosition).DestroySelf();
	}

	public Image GetBossHealth()
	{
		return bossHealthbar;
	}

	private void DisplayTime()
	{
		GameManager.Instance.SurviveTime -= Time.deltaTime;

		float minutes = Mathf.FloorToInt(GameManager.Instance.SurviveTime / 60);
		float seconds = Mathf.FloorToInt(GameManager.Instance.SurviveTime % 60);

		if (minutes <= 0) minutes = 0;
		if (seconds <= 0) seconds = 0;

		if (minutes == 0 && seconds == 0)
		{
			GameManager.Instance.SurviveTime = 0;
			timer.gameObject.SetActive(false);
			return;
		}

		timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
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
}
