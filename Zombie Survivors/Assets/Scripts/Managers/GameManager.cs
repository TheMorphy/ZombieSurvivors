using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private int SurviveTime = 10;
	private float timeElapsed = 0;

	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private LevelUI levelUI;
	
	private LevelSystem levelSystem;

	private PlayerDetailsSO playerDetails;
	private Player player;

	public static List<GameObject> createdEnemies = new List<GameObject>();

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
		StartCoroutine(enemySpawner.SpawnEnemies(timeElapsed));
	}

	private void Update()
	{
		timeElapsed += Time.deltaTime;
	}

	private void OnEnable()
	{
		levelUI.OnUpgradeSet += LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
	}

	private void OnDisable()
	{
		levelUI.OnUpgradeSet -= LevelUI_OnUpgradeSet;

		levelSystem.OnLevelChanged -= LevelSystem_OnLevelChanged;
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