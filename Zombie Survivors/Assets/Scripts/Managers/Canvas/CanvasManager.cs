using UnityEngine;
using UnityEngine.SceneManagement;

public enum CanvasType
{
	MainMenu,
	GameView,
	PauseScreen,
	Settings,
	EndScreen
}

public class CanvasManager : MonoBehaviour
{
	public static CanvasManager Instance;

	// All Posible Canvas Screens Can Be Referenced like this
	GameViewCanvasController gameViewController;
	// MainMenuCanvasController mainMenuCanvasController;
	// SettingsCanvasController settingsCanvasController;
	// etc...

	private void Awake()
	{
		if(Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		gameViewController = GetComponentInChildren<GameViewCanvasController>();
	}

	private void Start()
	{
		SwitchCanvas(CanvasType.GameView);
	}

	private void OnEnable()
	{
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	}

	private void OnDisable()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

	private void GameManager_OnGameStateChanged(GameState currentGameState)
	{
		switch (currentGameState)
		{
			case GameState.gameStarted:
				SwitchCanvas(CanvasType.GameView);
				break;
		}
	}

	public void SwitchCanvas(CanvasType canvasType)
	{
		if(gameViewController != null)
		{
			gameViewController.gameObject.SetActive(true);
		}
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public GameViewCanvasController GetActiveCanvas()
	{
		return gameViewController;
	}
}
