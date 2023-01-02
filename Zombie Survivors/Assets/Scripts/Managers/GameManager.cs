using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private int SurviveTime = 10;
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private TextMeshProUGUI messageTextTMP;

	[SerializeField] private int currentLevelListIndex = 0;
	[SerializeField] private List<GameObject> levelsList;

	private PlayerDetailsSO playerDetails;
	private Player player;

	[HideInInspector] public GameState gameState;
	[HideInInspector] public GameState previousGameState;

	public static List<GameObject> createdEnemies = new List<GameObject>();

	private void Awake()
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		// Instantiate player
		InstantiatePlayer();
	}

	private void OnEnable()
	{
		// Subscribe to player destroyed event
		player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
	}
	private void OnDisable()
	{
		// Unsubscribe from player destroyed event
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
	}
	private void Update()
	{
		HandleGameState();
	}

	/// <summary>
	/// Handle player destroyed event
	/// </summary>
	private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		previousGameState = gameState;
		gameState = GameState.gameLost;
	}

	private void InstantiatePlayer()
	{
		// Set player details - saved in current player scriptable object from the main menu
		playerDetails = GameResources.Instance.currentPlayer.playerDetails;

		// Instantiate player
		GameObject playerGameObject = Instantiate(playerDetails.PlayerPrefab);

		virtualCamera.Follow = playerGameObject.transform;
		virtualCamera.LookAt = playerGameObject.transform;

		// Initialize Player
		player = playerGameObject.GetComponent<Player>();

		player.Initialize(playerDetails);
	}

	/// <summary>
	/// Show level as being completed - load next level
	/// </summary>
	private IEnumerator LevelCompleted()
	{
		// Play next level
		gameState = GameState.playingLevel;

		// Wait 2 seconds
		yield return new WaitForSeconds(2f);

		// Display level completed
		yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! \n\nYOU'VE SURVIVED THIS LEVEL", Color.white, 5f));

		yield return null; // to avoid enter being detected twice

		// Increase index to next level
		currentLevelListIndex++;

		PlayLevel(currentLevelListIndex);
	}

	/// <summary>
	/// Handle game state
	/// </summary>
	private void HandleGameState()
	{
		// Handle game state
		switch (gameState)
		{
			case GameState.gameStarted:

				// Play first level
				PlayLevel(currentLevelListIndex);

				gameState = GameState.playingLevel;

				break;

			// While playing the level handle the tab key for the dungeon overview map.
			case GameState.playingLevel:

				if (Input.GetKeyDown(KeyCode.Escape))
				{
					PauseGameMenu();
				}

				break;

			// handle the level being completed
			case GameState.levelCompleted:

				// Display level completed text
				StartCoroutine(LevelCompleted());

				break;

			// handle the game being won (only trigger this once - test the previous game state to do this)
			case GameState.gameWon:

				if (previousGameState != GameState.gameWon)
					StartCoroutine(GameWon());

				break;

			// handle the game being lost (only trigger this once - test the previous game state to do this)
			case GameState.gameLost:

				if (previousGameState != GameState.gameLost)
				{
					StopAllCoroutines(); // Prevent messages if you clear the level just as you get killed
					StartCoroutine(GameLost());
				}

				break;

			// restart the game
			case GameState.restartGame:

				RestartGame();

				break;

			// if the game is paused and the pause menu showing, then pressing escape again will clear the pause menu
			case GameState.gamePaused:
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					PauseGameMenu();
				}
				break;
		}

	}
	/// <summary>
	/// Game Won
	/// </summary>
	private IEnumerator GameWon()
	{
		previousGameState = GameState.gameWon;

		// Disable player
		GetPlayer().playerController.DisablePlayer();

		// Wait 1 seconds
		yield return new WaitForSeconds(1f);

		yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

		// Set game state to restart game
		gameState = GameState.restartGame;
	}

	/// <summary>
	/// Game Lost
	/// </summary>
	private IEnumerator GameLost()
	{
		previousGameState = GameState.gameLost;

		// Disable player
		GetPlayer().playerController.DisablePlayer();

		// Wait 1 seconds
		yield return new WaitForSeconds(1f);

		// Disable enemies (FindObjectsOfType is resource hungry - but ok to use in this end of game situation)
		Enemy[] enemyArray = FindObjectsOfType<Enemy>();
		foreach (Enemy enemy in enemyArray)
		{
			enemy.gameObject.SetActive(false);
		}

		// Display game lost
		yield return StartCoroutine(DisplayMessageRoutine("BAD LUCK " + GameResources.Instance.currentPlayer.playerName + "! YOU HAVE SUCCUMBED TO THE DUNGEON", Color.white, 2f));

		yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

		// Set game state to restart game
		gameState = GameState.restartGame;
	}

	private void PlayLevel(int currentLevelListIndex)
	{
		LevelBuilder.Instance.GenerateLevel(levelsList[currentLevelListIndex]);

		// Set player roughly mid-room
		player.gameObject.transform.position = Vector3.zero;
	}

	private void PauseGameMenu()
	{
		if (gameState != GameState.gamePaused)
		{
			pauseMenu.SetActive(true);
			GetPlayer().playerController.DisablePlayer();

			// Set game state
			previousGameState = gameState;
			gameState = GameState.gamePaused;
		}
		else if (gameState == GameState.gamePaused)
		{
			pauseMenu.SetActive(false);
			GetPlayer().playerController.EnablePlayer();

			// Set game state
			gameState = previousGameState;
			previousGameState = GameState.gamePaused;

		}
	}

	/// <summary>
	/// Display the message text for displaySeconds  - if displaySeconds =0 then the message is displayed until the return key is pressed
	/// </summary>
	private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
	{
		// Set text
		messageTextTMP.SetText(text);
		messageTextTMP.color = textColor;

		// Display the message for the given time
		if (displaySeconds > 0f)
		{
			float timer = displaySeconds;

			while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
			{
				timer -= Time.deltaTime;
				yield return null;
			}
		}
		else
		// else display the message until the return button is pressed
		{
			while (!Input.GetKeyDown(KeyCode.Return))
			{
				yield return null;
			}
		}

		yield return null;

		// Clear text
		messageTextTMP.SetText("");
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