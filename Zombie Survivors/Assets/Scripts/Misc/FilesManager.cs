using System.Collections.Generic;
using UnityEngine;

public class FilesManager : MonoBehaviour
{
	private static FilesManager instace;

	private void Awake()
	{
		if (instace == null)
			instace = this;
		else
			Destroy(gameObject);

		DontDestroyOnLoad(this);

		CreateFiles();
	}

	private void CreateFiles()
	{
		SaveManager.CreateJsonFiles(new List<string>()
		{
			Settings.TRACKABLES,
			Settings.AIRDROPS,
			Settings.CARDS
		});
	}
}
