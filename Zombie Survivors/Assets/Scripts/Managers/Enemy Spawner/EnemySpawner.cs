using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
	public static EnemySpawner Instance;

	[SerializeField] private int NumberOfEnemeisToSpawn = 15;
	[SerializeField] private float spawnDelay = 1.5f;

	NavMeshTriangulation navMeshTriangulation;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	private void Start()
	{
		navMeshTriangulation = NavMesh.CalculateTriangulation();

		StartCoroutine(SpawnEnemies());
	}

	private IEnumerator SpawnEnemies()
	{
		int spawnedEnemeis = 0;

		while (spawnedEnemeis < NumberOfEnemeisToSpawn)
		{
			Spawn();
			spawnedEnemeis++;

			yield return new WaitForSeconds(spawnDelay);
		}
	}

	private void Spawn()
	{
		var enemeyObj = ObjectPool.Instance.SpawnFromPool("redEnemy", transform.position, Quaternion.identity);

		EnemyController enemy = enemeyObj.GetComponent<EnemyController>();

		int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);

		NavMeshHit Hit;

		if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out Hit, 2f, 1))
		{
			enemy.GetAgent().Warp(Hit.position);
			enemy.EnableNavMeshAgent();
		}
	}

}
