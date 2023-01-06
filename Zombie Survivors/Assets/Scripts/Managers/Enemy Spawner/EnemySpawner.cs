using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
	public List<EnemyDetailsSO> enemyDetails = new List<EnemyDetailsSO>();

	[SerializeField] private int NumberOfEnemeisToSpawn = 15;
	[SerializeField] private float spawnDelay = 1.5f;

	NavMeshTriangulation navMeshTriangulation;

	private Transform _playerTransform;

	public static List<Transform> activeEnemies = new List<Transform>();

	private void Awake()
	{
		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

		navMeshTriangulation = NavMesh.CalculateTriangulation();
	}

	public IEnumerator SpawnEnemies()
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
		GameObject enemyObj = Instantiate(enemyDetails[0].enemyPrefab);

		Enemy enemy = enemyObj.GetComponent<Enemy>();
		enemy.InitializeEnemy(enemyDetails[0]);

		Vector3 spawnPos = enemyObj.transform.position;

		// Get screen bounds. I don't know how it works, but I think it does
		var screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

		// Loops thorugh navMeshTriangulation, to get the vertex position, outside the camera bounds
		while ((spawnPos - _playerTransform.position).magnitude < (screenBounds - _playerTransform.position).magnitude)
		{
			int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
			spawnPos = navMeshTriangulation.vertices[vertexIndex];
		}

		NavMeshHit Hit;

		// Spawn the agent at the nav mesh vertex position
		if (NavMesh.SamplePosition(spawnPos, out Hit, 2f, 1))
		{
			enemy.enemyController.GetAgent().Warp(Hit.position);
		}
	}
}
