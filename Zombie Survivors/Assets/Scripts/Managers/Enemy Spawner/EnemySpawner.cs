using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private int NumberOfEnemeisToSpawn = 15;
	[SerializeField] private float spawnDelay = 1.5f;

	NavMeshTriangulation navMeshTriangulation;

	private Transform _playerTransform;

	private void Awake()
	{
		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Start()
	{
		// Calculates Nav Mesh triangulation, used to get mesh vertices
		navMeshTriangulation = NavMesh.CalculateTriangulation();

		StartCoroutine(SpawnEnemies());
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
		var enemeyObj = ObjectPool.Instance.SpawnFromPool("redEnemy", transform.position, Quaternion.identity);

		PlayerMovement.enemiesTransforms.Add(enemeyObj.transform);

		EnemyController enemy = enemeyObj.GetComponent<EnemyController>();

		Vector3 spawnPos = enemeyObj.transform.position;

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
			enemy.GetAgent().Warp(Hit.position);
		}
	}

	public void StopSpawning()
	{
		StopCoroutine(SpawnEnemies());
	}
}
