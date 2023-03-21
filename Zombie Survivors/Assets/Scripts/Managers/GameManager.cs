using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("CONTROLLERS")]
	[SerializeField] private EnemySpawner enemySpawner;

	[Space]
	[Header("GAMEPLAY")]
	[SerializeField] private int currentLevel = 0;
	[Tooltip("For how many minutes should the game be played")]
	public float SurviveTime = 10;
	[Tooltip("Delay in seconds, until enemies start spawning")]
	[SerializeField] private int prepareTime = 10;
	[Tooltip("After how many minutes should the airdrop be spawned")]
	[SerializeField] private float spawnAirdropTime = 0;
	[Tooltip("Every how many seconds should the EXPAND AREA be spawned")]
	[SerializeField] private float expandAreaDelay = 15f;

	private LevelSystem levelSystem;
	private PlayerDetailsSO playerDetails;
	private Player player;

	private const float SPAWN_MARGIN = 5f; // Distance in units from the navmesh edge. To make sure that circle doesn't spawn too close to edge
	private bool airdropDropped = false;
	private bool bossSpawned = false;

	public static event Action<GameState> OnGameStateChanged;
	[HideInInspector] public GameState GameState { get; private set; }
	
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		levelSystem = new LevelSystem();
	}

	private void Start()
	{
		SurviveTime *= 60;
		spawnAirdropTime *= 60;

		ChangeGameState(GameState.GameStarted);
	}

	private void Update()
	{
		if(Mathf.CeilToInt(SurviveTime - TimeTracker.Instance.GameTime) == spawnAirdropTime && !airdropDropped)
		{
			SpawnAirdrop();
		}

		if(TimeTracker.Instance.GameTime <= 0 && !bossSpawned)
		{
			SpawnBoss();
		}
	}

	private void SpawnBoss()
	{
		enemySpawner.SpawnBoss(currentLevel);
		bossSpawned = true;
	}

	public async void ChangeGameState(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.GameStarted:

				InstantiatePlayer();

				break;
			case GameState.PlayingLevel:

				TimeTracker.Instance.TrackGameTime(SurviveTime);

				await Utilities.Wait(3);

				levelSystem.AddExperience(100);

				await Utilities.Wait(prepareTime);

				EnableSpawners();

				break;
			case GameState.BossFight:

				break;

			case GameState.GameWon:

				break;

			case GameState.GameLost:
				DisableSpawners();
				GameState = GameState.GamePaused;

				break;

			case GameState.GamePaused:

				break;
		}

		this.GameState = gameState;
		OnGameStateChanged?.Invoke(gameState);
	}

	/// <summary>
	/// When player gets fully initialized, start the level
	/// </summary>
	private void Player_OnPlayerInitialized()
	{
		ChangeGameState(GameState.PlayingLevel);
	}

	private void InstantiatePlayer()
	{
		// Set player details - saved in current player scriptable object from the main menu
		playerDetails = GameResources.Instance.CurrentPlayer.playerDetails;

		// Instantiate player
		GameObject playerGameObject = Instantiate(playerDetails.PlayerPrefab);

		// Initialize Player
		player = playerGameObject.GetComponent<Player>();

		player.OnPlayerInitialized += Player_OnPlayerInitialized;

		player.Initialize(playerDetails);
	}

	private void EnableSpawners()
	{
		enemySpawner.Player = player;
		StartCoroutine(enemySpawner.SpawnEnemies());
		StartCoroutine(SpawnNewExpandAreaAtRandomPosition());
	}

	public void DisableSpawners()
	{
		StopCoroutine(enemySpawner.SpawnEnemies());
		StopCoroutine(SpawnNewExpandAreaAtRandomPosition());
	}

	private void SpawnAirdrop()
	{
		airdropDropped = true;
		var airdrops = GameResources.Instance.Airdrops;
		int index = UnityEngine.Random.Range(0, airdrops.Count);

		GameObject airdrop = Instantiate(airdrops[index].AirdropPackage);
		airdrop.transform.position = GetRandomSpawnPositionGround(4);

		airdrop.GetComponent<Airdrop>().InitializeAirdrop(airdrops[index]);

		StaticEvents.CallAirdropSpawnedEvent(airdrop.transform.position);
	}

	private IEnumerator SpawnNewExpandAreaAtRandomPosition()
	{
		WaitForSeconds spawnDelay = new WaitForSeconds(expandAreaDelay);

		while (SurviveTime > 0)
		{
			yield return spawnDelay;

			Vector3 spawnPos = GetRandomSpawnPositionGround(SPAWN_MARGIN);

			GameObject circle = Instantiate(GameResources.Instance.MultiplicationCircle);
			circle.transform.position = spawnPos;

			StaticEvents.CallCircleSpawnedEvent(spawnPos);
		}
	}
	/// <summary>
	///  Gets random position on the nav mesh. 
	///  Also takes into account a margin to prevent object from spawning directly on the edge
	/// </summary>
	/// <param name="spawnMargin">Distance from ground sides in units</param>
	public Vector3 GetRandomSpawnPositionGround(float spawnMargin = 0)
	{
		NavMeshHit hit;
		Vector3 randomPoint;
		NavMeshTriangulation triangulation = enemySpawner.GetNavMeshTriangulation();

		do
		{
			randomPoint = new Vector3(
				UnityEngine.Random.Range(triangulation.vertices[0].x + spawnMargin, triangulation.vertices[2].x - spawnMargin),
				0,
				UnityEngine.Random.Range(triangulation.vertices[0].z + spawnMargin, triangulation.vertices[2].z - spawnMargin));
		} while (!NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas));
		return hit.position;
	}

	public void SpawnEvacuationArea()
	{
		ChangeGameState(GameState.Evacuating);

		Instantiate(GameResources.Instance.EvacuationArea, Vector3.zero, Quaternion.identity);
	}

	public void Evacuate(Transform evacuationZonePosition)
	{
		player.PlayerController.StopPlayer();

		CameraController.Instance.SetInitialTarget(evacuationZonePosition);

		StartCoroutine(player.SquadControl.MoveTransformsToPosition(evacuationZonePosition.position));
	}

	public Player GetPlayer()
	{
		return player;
	}

	public LevelSystem GetLevelSystem()
	{
		return levelSystem;
	}

	public void RevivePlayer()
	{
		ChangeGameState(GameState.PlayingLevel);
		EnableSpawners();

		player.SquadControl.CreateFirstComrade();
		player.PlayerController.SetPlayerToAlive();
	}
}

public enum GameState
{
	GameStarted,
	RestartGame,
	PlayingLevel,
	BossFight,
	Evacuating,
	GameWon,
	GameLost,
	GamePaused
}