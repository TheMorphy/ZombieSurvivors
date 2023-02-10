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
	[HideInInspector] public GameViewCanvasController gameViewController;

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
			case GameState.gamePaused:
				SwitchCanvas(CanvasType.PauseScreen);
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

	public void StartGame()
	{
		SlotsController.Instance.GetOccupiedSlots().ForEach(slot => 
		{ 
			if (slot.isTimerRunning) 
			{ 
				slot.SaveOpeniningTime(); 
			} 
		});

		SceneManager.LoadScene(1);
	}

	public GameViewCanvasController GetActiveCanvas()
	{
		return gameViewController;
	}
}
