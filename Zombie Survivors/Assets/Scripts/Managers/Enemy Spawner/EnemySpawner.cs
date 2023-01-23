using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
	public List<EnemyDetailsSO> Enemies = new List<EnemyDetailsSO>();
	public ScalingConfigurationSO EnemiesScalingConfiguration;

	[SerializeField] private int NumberOfEnemies = 15;
	[SerializeField] private float spawnDelay = 1.5f;
	//[SerializeField] private bool spawnEndlessly = false;

	NavMeshTriangulation navTriangulation;
	public static List<Transform> activeEnemies = new List<Transform>();

	[Space]
	[Header("Read At Runtime")]
	[Space]
	[SerializeField]
	private int Level = 0;

	[SerializeField]
	private List<EnemyDetailsSO> scaledEnemies = new List<EnemyDetailsSO>();

	private int enemiesAlive = 0;
	private int enemiesSpawned = 0;
	private int initialEnemiesToSpawn = 0;
	private float InitialSpawnDelay = 0;

	private void Awake()
	{
		navTriangulation = NavMesh.CalculateTriangulation();

		initialEnemiesToSpawn = NumberOfEnemies;
		InitialSpawnDelay = spawnDelay;
	}

	private void Start()
	{
		for (int i = 0; i < Enemies.Count; i++)
		{
			scaledEnemies.Add(Enemies[i].ScaleUpLevel(EnemiesScalingConfiguration, 0));
		}
	}

	public IEnumerator SpawnEnemies()
	{
		Level++;
		enemiesAlive = 0;
		enemiesSpawned = 0;

		for (int i = 0; i < Enemies.Count; i++)
		{
			scaledEnemies[i] = Enemies[i].ScaleUpLevel(EnemiesScalingConfiguration, Level);
		}

		WaitForSeconds Wait = new WaitForSeconds(spawnDelay);
		while (enemiesSpawned < NumberOfEnemies)
		{
			Spawn();

			yield return Wait;
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

		GameObject enemyObj = Instantiate(scaledEnemies[0].enemyPrefab, randomPos, Quaternion.identity);

		Enemy enemy = enemyObj.GetComponent<Enemy>();
		enemy.enemyController.GetAgent().enabled = false;

		enemy.InitializeEnemy(scaledEnemies[0]);

		enemy.destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;

		enemiesSpawned++;
		enemiesAlive++;
	}

	Vector3 RandomNavmeshLocation(Vector3 origin, float range)
	{
		NavMeshHit hit;
		Vector3 randomDirection = Random.insideUnitSphere * range;
		Vector3 randomPos = origin + randomDirection;
		NavMesh.SamplePosition(randomPos, out hit, range, 1);
		int randomTriangleIndex = Random.Range(0, navTriangulation.indices.Length / 3);
		Vector3[] triangle = new Vector3[3];
		for (int i = 0; i < 3; i++)
		{
			triangle[i] = navTriangulation.vertices[navTriangulation.indices[randomTriangleIndex * 3 + i]];
		}
		randomPos = hit.position;
		return randomPos;
	}

	private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		if(destroyedEventArgs.playerDied == false)
		{
			enemiesAlive--;

			if (enemiesAlive == 0 && enemiesSpawned == NumberOfEnemies)
			{
				ScaleUpSpawns();
				StartCoroutine(SpawnEnemies());
			}
		}
	}

	public void ScaleUpSpawns()
	{
		NumberOfEnemies = Mathf.FloorToInt(initialEnemiesToSpawn * EnemiesScalingConfiguration.SpawnCountCurve.Evaluate(Level + 1));
		spawnDelay = InitialSpawnDelay * EnemiesScalingConfiguration.SpawnRateCurve.Evaluate(Level + 1);
	}
}
