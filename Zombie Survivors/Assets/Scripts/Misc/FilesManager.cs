using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesManager : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(this);

		CreateFiles();
	}

	private void CreateFiles()
	{
		SaveManager.CreateEmptyJSONFiles(new List<string>()
		{
			Settings.TRACKABLES,
			Settings.ACTIVE_UPGRADES,
			Settings.AIRDROPS,
			Settings.ALL_CARDS
		});
	}
}
