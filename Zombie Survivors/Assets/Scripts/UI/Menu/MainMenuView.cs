using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : View
{
	[SerializeField] private Button startGame;

	public override void Initialize()
	{
		startGame.onClick.AddListener(() => ViewManager.ShowLast());
	}
}
