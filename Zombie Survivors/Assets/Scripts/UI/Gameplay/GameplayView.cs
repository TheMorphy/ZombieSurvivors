using UnityEngine;
using UnityEngine.UI;

public class GameplayView : View
{
	[SerializeField] private Button backToMenuBtn;

	public override void Initialize()
	{
		backToMenuBtn.onClick.AddListener(() => ViewManager.Show<MainMenuView>());
	}
}
