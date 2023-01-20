using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private float SurviveTime = 10;
	private float timeElapsed = 0;

	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private CinemachineTargetGroup targetGroup;
	[SerializeField] private LevelUI levelUI;

	private LevelSystem levelSystem;

	private PlayerDetailsSO playerDetails;
	private Player player;

	public static List<GameObject> createdEnemies = new List<GameObject>();

	// To make sure that circle doesn't spawn on the edge
	private float spawnMargin = 5;
	private Vector3 spawnPos;
	private Bounds groundBounds;

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

		// Instantiate player
		InstantiatePlayer();
	}

	private void LateUpdate()
	{
		targetGroup.transform.position = player.transform.position;
	}

	private void Update()
	{
		SurviveTime -= Time.deltaTime;
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
		StartCoroutine(enemySpawner.SpawnEnemies());
		StartCoroutine(SpawnNewExpandAreaAtRandomPosition());
	}


	public void AddTargetToCamera(Transform transform, float weight = 1, float radius = 1)
	{
		targetGroup.AddMember(transform, weight, radius);
	}

	public void RemoveTargetFromCamera(Transform transform)
	{
		targetGroup.RemoveMember(transform);
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
	
	private IEnumerator SpawnNewExpandAreaAtRandomPosition()
	{
		while (SurviveTime > 0)
		{
			spawnPos = GetRandomSpawnPositionGround(spawnMargin);

			GameObject circle = Instantiate(GameResources.Instance.MultiplicationCircle);
			circle.transform.position = spawnPos;

			StaticEvents.CallCircleSpawnedEvent(spawnPos);
			
			yield return new WaitForSeconds(10);
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
		player = playerGameObject.GetComponentInChildren<Player>();

		player.Initialize(playerDetails);
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
	levelCompleted,
	gameWon,
	gameLost,
	gamePaused
}