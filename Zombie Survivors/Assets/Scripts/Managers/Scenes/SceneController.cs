using UnityEngine.SceneManagement;
using UnityEngine;

[DisallowMultipleComponent]
public class SceneController : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
