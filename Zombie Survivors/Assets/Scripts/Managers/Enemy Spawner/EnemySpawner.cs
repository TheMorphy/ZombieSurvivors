using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
	public List<EnemyDetailsSO> Enemies = new List<EnemyDetailsSO>();
	public ScalingConfigurationSO ScalingConfiguration;

	[SerializeField] private int NumberOfEnemies = 15;
	[SerializeField] private float spawnDelay = 1.5f;
	[SerializeField] private bool spawnEndlessly = false;

	[Space]
	[Header("Bosses")]
	[Tooltip("List position represents level number")]
	public List<EnemyDetailsSO> Bosses = new List<EnemyDetailsSO>();
	[SerializeField] private BossHealthUI bossHealthUI;

	NavMeshTriangulation navTriangulation;
	public static List<Transform> activeEnemies = new List<Transform>();

	[Space]
	[Header("Read At Runtime (Readonly)")]
	[Space]
	private int Level = 0;
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
			if(spawnEndlessly == false)
			{
				scaledEnemies.Add(Enemies[i].ScaleUpEnemiesByLevel(ScalingConfiguration, 0));
			}
			else
			{
				scaledEnemies.Add(Enemies[i]);
			}
		}
	}

	/// <summary>
	/// So at the moment it spawns enemies in waves and each wave gets stronger.
	/// </summary>
	public IEnumerator SpawnEnemies()
	{
		if(spawnEndlessly == false)
		{
			Level++;
			enemiesAlive = 0;
			enemiesSpawned = 0;

			for (int i = 0; i < Enemies.Count; i++)
			{
				scaledEnemies.Add(Enemies[i].ScaleUpEnemiesByLevel(ScalingConfiguration, Level));
			}
		}
		
		WaitForSeconds Wait = new WaitForSeconds(spawnDelay);
		while (enemiesSpawned < NumberOfEnemies || spawnEndlessly)
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
			// Find a random point on the navmesh within 50 units
			float spawnRadius = 50f;
			randomPos = RandomNavmeshLocation(Camera.main.transform.position, spawnRadius);
			// Check if the enemy is within the camera's view
			screenPos = Camera.main.WorldToScreenPoint(randomPos);
		}
		while (screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height);

		GameObject enemyObj = Instantiate(scaledEnemies[0].enemyPrefab, randomPos, Quaternion.identity);

		Enemy enemy = enemyObj.GetComponent<Enemy>();
		enemy.enemyController.GetAgent().enabled = false;

		enemy.InitializeEnemy(scaledEnemies[0]);

		if (spawnEndlessly)
		{
			float timeElapsed = GameManager.Instance.GetElapsedTime();
			for (int i = 0; i < scaledEnemies.Count; i++)
			{
				scaledEnemies[i].ScaleUpEnemiesByTime(ScalingConfiguration, timeElapsed);
			}
			
			ScaleUpSpawns(timeElapsed);
		}
		else
		{
			enemy.destroyedEvent.OnDestroyed += DestroyedEvent_OnEnemyDestroyed;

			enemiesSpawned++;
			enemiesAlive++;
		}
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

	private void DestroyedEvent_OnEnemyDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
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

	public void SpawnBoss(int levelIndex)
	{
		bossHealthUI.GetUITransform().gameObject.SetActive(true);

		Enemy boss = Instantiate(Bosses[levelIndex].enemyPrefab, Vector3.zero, Quaternion.identity).GetComponent<Enemy>();
		boss.InitializeEnemy(Bosses[levelIndex]);
		boss.InitializeBossHealthbar(bossHealthUI.GetHealthbar());

		boss.destroyedEvent.OnDestroyed += DestroyedEvent_OnBossDestroyed;
	}

	private void DestroyedEvent_OnBossDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		bossHealthUI.GetUITransform().gameObject.SetActive(false);

		GameManager.Instance.SpawnEvacuationArea();
	}

	public void ScaleUpSpawns()
	{
		NumberOfEnemies = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(Level + 1));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(Level + 1);
	}

	public void ScaleUpSpawns(float time)
	{
		NumberOfEnemies = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(time));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(time);
	}
}
