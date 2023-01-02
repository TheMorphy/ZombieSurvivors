using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class LevelBuilder : MonoBehaviour
{
	public static LevelBuilder Instance;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void GenerateLevel(GameObject currentLevel)
	{
		Instantiate(currentLevel);
	}
}
