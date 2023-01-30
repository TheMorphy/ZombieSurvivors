using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Controllers")]
	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private SceneController sceneController;

	[Space]
	[Header("Base Info")]
	[SerializeField] private int currentLevel = 0;
	public float SurviveTime = 10;
	private float timeElapsed = 0;
	
	private LevelSystem levelSystem;
	private PlayerDetailsSO playerDetails;
	private Player player;

	// To make sure that circle doesn't spawn on the edge
	private float spawnMargin = 5;
	private Vector3 spawnPos;
	private Bounds groundBounds;
	private bool airdropDropped = false;

	public static event Action<GameState> OnGameStateChanged;
	[HideInInspector] public GameState gameState;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		levelSystem = new LevelSystem();

		groundBounds = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>().bounds;
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

				break;
			case GameState.playingLevel:

				timeElapsed += Time.deltaTime;

				// Spawns Airdrop after 1min
				if (Mathf.FloorToInt(timeElapsed) == 8 && !airdropDropped)
				{
					SpawnAirdrop();
				}

				if (SurviveTime <= 0)
				{
					StopAllCoroutines();
					enemySpawner.SpawnBoss(currentLevel);
				}

				break;
			case GameState.bossFight:

				break;

			case GameState.gameWon:

				break;
		}
	}

	private void OnEnable()
	{
		levelSystem.OnLevelUp += LevelSystem_OnLevelUp;

		StaticEvents.OnPlayerInitialized += StaticEvents_OnPlayerInitialized;
	}

	private void OnDisable()
	{
		levelSystem.OnLevelUp -= LevelSystem_OnLevelUp;

		StaticEvents.OnPlayerInitialized -= StaticEvents_OnPlayerInitialized;
	}

	/// <summary>
	/// When player gets fully initialized, start spawning enemeis and multiplication circles
	/// </summary>
	private void StaticEvents_OnPlayerInitialized(ComradeBoardedEventArgs playerInitializedEventArgs)
	{
		CallGameStateChangedEvent(GameState.gameStarted);
	}

	private void InstantiatePlayer()
	{
		// Set player details - saved in current player scriptable object from the main menu
		playerDetails = GameResources.Instance.CurrentPlayer.playerDetails;

		// Instantiate player
		GameObject playerGameObject = Instantiate(playerDetails.PlayerPrefab);

		// Initialize Player
		player = playerGameObject.GetComponent<Player>();

		player.Initialize(playerDetails);

		Camera.main.GetComponent<CameraController>().SetTarget(playerGameObject.transform);

	}

	private void LevelSystem_OnLevelUp(object sender, EventArgs e)
	{
		player.playerController.DisablePlayerMovement();
	}

	private void EnableSpawners()
	{
		StartCoroutine(enemySpawner.SpawnEnemies());
		StartCoroutine(SpawnNewExpandAreaAtRandomPosition());

		CallGameStateChangedEvent(GameState.playingLevel);
	}

	public void CallGameStateChangedEvent(GameState gameState)
	{
		this.gameState = gameState;
		OnGameStateChanged?.Invoke(gameState);
	}

	private void SpawnAirdrop()
	{
		airdropDropped = true;
		var airdrops = GameResources.Instance.Airdrops;
		int index = UnityEngine.Random.Range(0, airdrops.Count);

		GameObject airdrop = Instantiate(airdrops[index].airdropPackage);
		airdrop.transform.position = GetRandomSpawnPositionGround(4);

		airdrop.GetComponent<AirdropController>().InitializeAirdrop(airdrops[index]);

		StaticEvents.CallAirdropSpawnedEvent(airdrop.transform.position);
	}

	public float GetElapsedTime()
	{
		return timeElapsed;
	}

	private IEnumerator SpawnNewExpandAreaAtRandomPosition()
	{
		float delay = 15;
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
	/// <summary>
	///  Gets random position on the ground. 
	///  Also takes into account a margin to prevent object from spawning directly on the edge
	/// </summary>
	/// <param name="spawnMargin">Distance from ground sides in units</param>
	public Vector3 GetRandomSpawnPositionGround(float spawnMargin = 0)
	{
		return new Vector3(
			UnityEngine.Random.Range(groundBounds.min.x + spawnMargin, groundBounds.max.x - spawnMargin),
			0,
			UnityEngine.Random.Range(groundBounds.min.z + spawnMargin, groundBounds.max.z - spawnMargin));
	}

	public void SpawnEvacuationArea()
	{
		CallGameStateChangedEvent(GameState.evacuating);

		Instantiate(GameResources.Instance.EvacuationArea, Vector3.zero, Quaternion.identity);
	}

	public void Evacuate(Vector3 evacuationZonePosition)
	{
		//Camera.main.GetComponent<CameraController>().SetTarget(evacuationZonePosition);

		player.playerController.StopPlayer();

		StartCoroutine(player.squadControl.MoveTransformsToPosition(evacuationZonePosition));
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
	evacuating,
	gameWon,
	gameLost,
	gamePaused
}