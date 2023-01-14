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
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
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

		levelSystem = new LevelSystem();

		levelUI.SetLevelSystem(levelSystem);

		// Instantiate player
		InstantiatePlayer();
	}

	private void Start()
	{
		SurviveTime *= 60;

		groundBounds = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>().bounds;

		StartCoroutine(enemySpawner.SpawnEnemies(timeElapsed));

		StartCoroutine(SpawnNewExpandAreaAtRandomPosition());
	}

	private void Update()
	{
		SurviveTime -= Time.deltaTime;
	}

	private void OnEnable()
	{
		levelUI.OnUpgradeSet += LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;

		//player.squadControl.OnSquadIncrease += SquadControl_OnSquadIncrease;

		//player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
	}

	private void OnDisable()
	{
		levelUI.OnUpgradeSet -= LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;

		//player.squadControl.OnSquadIncrease -= SquadControl_OnSquadIncrease;

		//player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
	}

	//private void SquadControl_OnSquadIncrease(SquadControl arg1, int arg2)
	//{
	//	arg1.gameObject.SetActive(false);

	//	SpawnNewExpandArea = true;
	//}

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
			spawnPos = new Vector3(
			Random.Range(groundBounds.min.x + spawnMargin, groundBounds.max.x - spawnMargin),
			0,
			Random.Range(groundBounds.min.z + spawnMargin, groundBounds.max.z - spawnMargin));

			GameObject circle = Instantiate(GameResources.Instance.MultiplicationCircle);
			circle.transform.position = spawnPos;
			
			yield return new WaitForSeconds(60);
		}
	}

	//private void Player_OnDestroyed(object sender, System.EventArgs e)
	//{
	//	//previousGameState = gameState;
	//	//gameState = GameState.gameLost;
	//}

	private void InstantiatePlayer()
	{
		// Set player details - saved in current player scriptable object from the main menu
		playerDetails = GameResources.Instance.CurrentPlayer.playerDetails;

		// Instantiate player
		GameObject playerGameObject = Instantiate(playerDetails.PlayerPrefab);

		virtualCamera.Follow = playerGameObject.transform;
		virtualCamera.LookAt = playerGameObject.transform;

		// Initialize Player
		player = playerGameObject.GetComponentInChildren<Player>();

		player.Initialize(playerDetails);
	}

	/// <summary>
	/// Restart the game
	/// </summary>
	private void RestartGame()
	{
		SceneManager.LoadScene("MainMenuScene");
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