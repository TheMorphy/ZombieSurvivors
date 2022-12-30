using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
	public static EnemySpawner Instance;

	[SerializeField] private int NumberOfEnemeisToSpawn = 15;
	[SerializeField] private float spawnDelay = 1.5f;

	NavMeshTriangulation navMeshTriangulation;

	private Transform _playerTransform;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Start()
	{
		navMeshTriangulation = NavMesh.CalculateTriangulation();

		StartCoroutine(SpawnEnemies());
	}

	private IEnumerator SpawnEnemies()
	{
		int spawnedEnemies = 0;

		while (spawnedEnemies < NumberOfEnemeisToSpawn)
		{
			Spawn();
			spawnedEnemies++;

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	private void Spawn()
	{
		var enemeyObj = ObjectPool.Instance.SpawnFromPool("redEnemy", transform.position, Quaternion.identity);

		PlayerController.enemiesTransforms.Add(enemeyObj.transform);

		EnemyController enemy = enemeyObj.GetComponent<EnemyController>();

		Vector3 spawnPos = enemeyObj.transform.position;

		var screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

		Debug.Log(screenBounds);

		while ((spawnPos - _playerTransform.position).magnitude < (screenBounds - _playerTransform.position).magnitude)
		{
			int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
			spawnPos = navMeshTriangulation.vertices[vertexIndex];
		}

		NavMeshHit Hit;

		if (NavMesh.SamplePosition(spawnPos, out Hit, 2f, 1))
		{
			enemy.GetAgent().Warp(Hit.position);
		}
	}

}
