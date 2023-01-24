using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private float SurviveTime = 10;
	[SerializeField] private int currentLevel = 0;
	[SerializeField] private float timeElapsed = 0;

	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private UpgradesUI levelUI;
	[SerializeField] private TextMeshProUGUI timerText;

	private LevelSystem levelSystem;

	private PlayerDetailsSO playerDetails;
	private Player player;

	// To make sure that circle doesn't spawn on the edge
	private float spawnMargin = 5;
	private Vector3 spawnPos;
	private Bounds groundBounds;

	[HideInInspector] public GameState gameState;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		groundBounds = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>().bounds;

		levelSystem = new LevelSystem();

		levelUI.SetLevelSystem(levelSystem);
	}

	private void Start()
	{
		SurviveTime *= 60;

		InstantiatePlayer();
	}

	private void Update()
	{
		switch (gameState)
		{
			case GameState.gameStarted:

				EnableSpawners();
				gameState = GameState.playingLevel;

				break;
			case GameState.playingLevel:

				timeElapsed += Time.deltaTime;
				DisplayTime();

				if(SurviveTime == 0)
				{
					StopAllCoroutines();
					gameState = GameState.bossFight;
				}
					
				break;
			case GameState.bossFight:

				enemySpawner.SpawnBoss(currentLevel);
				gameState = GameState.engagngBoss;

				break;

			case GameState.engagngBoss:


				break;
			case GameState.evacuating:


				break;
		}
	}
	private void OnEnable()
	{
		levelUI.OnUpgradeSet += LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

		StaticEvents.OnPlayerInitialized += StaticEvents_OnPlayerInitialized;
	}

	private void OnDisable()
	{
		levelUI.OnUpgradeSet -= LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;

		StaticEvents.OnPlayerInitialized -= StaticEvents_OnPlayerInitialized;
	}

	/// <summary>
	/// When player gets fully initialized, start spawning enemeis and multiplication circles
	/// </summary>
	private void StaticEvents_OnPlayerInitialized(PlayerInitializedEventArgs playerInitializedEventArgs)
	{
		gameState = GameState.gameStarted;
	}

	private void EnableSpawners()
	{
		StartCoroutine(enemySpawner.SpawnEnemies());
		StartCoroutine(SpawnNewExpandAreaAtRandomPosition());
	}

	private void LevelUI_OnUpgradeSet()
	{
		levelUI.gameObject.SetActive(false);
		player.playerController.EnablePlayerMovement();
	}

	private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
	{
		player.playerController.DisablePlayerMovement();
		levelUI.gameObject.SetActive(true);
	}

	public float GetElapsedTime()
	{
		return timeElapsed;
	}

	void DisplayTime()
	{
		SurviveTime -= Time.deltaTime;

		float minutes = Mathf.FloorToInt(SurviveTime / 60);
		float seconds = Mathf.FloorToInt(SurviveTime % 60);

		if(minutes <= 0) minutes = 0;
		if(seconds <= 0) seconds = 0;

		if(minutes == 0 && seconds == 0)
		{
			SurviveTime = 0;
			timerText.gameObject.SetActive(false);
			return;
		} 

		timerText.text = minutes + ":" + seconds;
	}

	private IEnumerator SpawnNewExpandAreaAtRandomPosition()
	{
		float delay = 30;
		WaitForSeconds spawnDelay = new WaitForSeconds(delay);

		while (SurviveTime > 0)
		{
			spawnPos = GetRandomSpawnPositionGround(spawnMargin);

			GameObject circle = Instantiate(GameResources.Instance.MultiplicationCircle);
			circle.transform.position = spawnPos;

			StaticEvents.CallCircleSpawnedEvent(spawnPos);

			yield return spawnDelay;
		}
	}

	public Vector3 GetRandomSpawnPositionGround(float spawnMargin = 0)
	{
		return new Vector3(
			Random.Range(groundBounds.min.x + spawnMargin, groundBounds.max.x - spawnMargin),
			0,
			Random.Range(groundBounds.min.z + spawnMargin, groundBounds.max.z - spawnMargin));
	}

	private void InstantiatePlayer()
	{
		// Set player details - saved in current player scriptable object from the main menu
		playerDetails = GameResources.Instance.CurrentPlayer.playerDetails;

		// Instantiate player
		GameObject playerGameObject = Instantiate(playerDetails.PlayerPrefab);

		// Initialize Player
		player = playerGameObject.GetComponent<Player>();

		Camera.main.GetComponent<CameraController>().Player = player;

		player.Initialize(playerDetails);
	}

	public void SpawnEvacuationArea()
	{
		gameState = GameState.evacuating;

		Instantiate(GameResources.Instance.EvacuationArea, Vector3.zero, Quaternion.identity);
	}

	public void Evacuate(Vector3 evacuationZonePosition)
	{
		player.playerController.StopPlayer();

		player.squadControl.DisableComrades();

		var arangedComrades = SquadControl.ComradesTransforms
			.OrderBy(x => Vector3.Distance(x.transform.position, evacuationZonePosition)).ToList();

		print(arangedComrades.Count);

		StartCoroutine(MoveTransformsToPosition(arangedComrades, evacuationZonePosition));
	}

	IEnumerator MoveTransformsToPosition(List<Transform> transforms, Vector3 position)
	{
		// A bit of delay before boarding
		yield return new WaitForSeconds(1.5f);

		foreach (var t in transforms)
		{
			yield return t.DOMove(position, 0.7f).WaitForCompletion();
			t.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Restart the game
	/// </summary>
	private void RestartGame()
	{
		SceneManager.LoadScene("MainMenu");
	}

	/// <summary>
	/// Get the player
	/// </summary>
	public Player GetPlayer()
	{
		return player;
	}

	public LevelSystem GetLevelSystem()
	{
		return levelSystem;
	}
}

public enum GameState
{
	gameStarted,
	restartGame,
	playingLevel,
	bossFight,
	engagngBoss,
	preparingForEvacuation,
	evacuating,
	gameWon,
	gameLost,
	gamePaused
}