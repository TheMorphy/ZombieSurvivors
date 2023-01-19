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
	[SerializeField] private bool spawnEndlessly = false;

	private int spawnedEnemies = 0;

	NavMeshTriangulation navTriangulation;

	public static List<Transform> activeEnemies = new List<Transform>();

	private void Awake()
	{
		navTriangulation = NavMesh.CalculateTriangulation();
	}

	private void Start()
	{
		StartCoroutine(SpawnEnemies());
	}

	public IEnumerator SpawnEnemies()
	{
		while (spawnedEnemies < NumberOfEnemeisToSpawn || spawnEndlessly)
		{
			Spawn();
			
			yield return new WaitForSeconds(spawnDelay);
		}
	}

	private void Spawn()
	{
		Vector3 randomPos;
		Vector3 screenPos;

		do
		{
			// Find a random point on the navmesh
			randomPos = RandomNavmeshLocation(Camera.main.transform.position, 50f);
			// Check if the enemy is within the camera's view
			screenPos = Camera.main.WorldToScreenPoint(randomPos);
		}
		while (screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height);

		GameObject enemyObj = Instantiate(enemyDetails[0].enemyPrefab, randomPos, Quaternion.identity);

		Enemy enemy = enemyObj.GetComponent<Enemy>();
		enemy.enemyController.GetAgent().enabled = false;

		enemy.InitializeEnemy(enemyDetails[0]);

		spawnedEnemies++;
	}

	Vector3 RandomNavmeshLocation(Vector3 origin, float range)
	{
		NavMeshHit hit;
		Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * range;
		Vector3 randomPos = origin + randomDirection;
		NavMesh.SamplePosition(randomPos, out hit, range, 1);
		int randomTriangleIndex = UnityEngine.Random.Range(0, navTriangulation.indices.Length / 3);
		Vector3[] triangle = new Vector3[3];
		for (int i = 0; i < 3; i++)
		{
			triangle[i] = navTriangulation.vertices[navTriangulation.indices[randomTriangleIndex * 3 + i]];
		}
		randomPos = hit.position;
		return randomPos;
	}



	//public Vector3 GetRandSpawnPosOnMesh()
	//{
	//	int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
	//	return navMeshTriangulation.vertices[vertexIndex];
	//}
}
