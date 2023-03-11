using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
	public List<EnemyDetailsSO> Enemies = new List<EnemyDetailsSO>();
	public ScalingConfigurationSO ScalingConfiguration;

	[SerializeField] private int StartEnemyCount = 15;
	[SerializeField] private float spawnDelay = 1.5f;
	//[SerializeField] private bool spawnEndlessly = false;

	[Space]
	[Header("Bosses")]
	[Tooltip("List position represents level number")]
	public List<EnemyDetailsSO> Bosses = new List<EnemyDetailsSO>();

	NavMeshTriangulation navTriangulation;
	public static List<Transform> ActiveEnemies;

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
		initialEnemiesToSpawn = StartEnemyCount;
		InitialSpawnDelay = spawnDelay;
	}

	private void Start()
	{
		ActiveEnemies = new List<Transform>();

		for (int i = 0; i < Enemies.Count; i++)
		{
			scaledEnemies.Add(Enemies[i].ScaleUpEnemies(ScalingConfiguration, 0));
		}
	}

	/// <summary>
	/// So at the moment it spawns enemies in waves and each wave gets stronger.
	/// </summary>
	public IEnumerator SpawnEnemies()
	{
		Level++;
		enemiesAlive = 0;
		enemiesSpawned = 0;

		for (int i = 0; i < Enemies.Count; i++)
		{
			scaledEnemies.Add(Enemies[i].ScaleUpEnemies(ScalingConfiguration, Level));
		}

		WaitForSeconds Wait = new WaitForSeconds(spawnDelay);
		while (enemiesSpawned < StartEnemyCount)
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

		enemy.destroyedEvent.OnDestroyed += DestroyedEvent_OnEnemyDestroyed;

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

	private void DestroyedEvent_OnEnemyDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		if(destroyedEventArgs.playerDied == false)
		{
			enemiesAlive--;

			if (enemiesAlive == 0 && enemiesSpawned == StartEnemyCount)
			{
				if(GameManager.Instance.GameState == GameState.PlayingLevel)
				{
					ScaleUpSpawns();
					StartCoroutine(SpawnEnemies());
				}
			}
		}
	}

	public void SpawnBoss(int levelIndex)
	{
		AudioManager.Instance.PlayMusicWithCrossFade(SoundTitle.BossFight_Theme);

		GameManager.Instance.ChangeGameState(GameState.BossFight);

		Vector3 bossSpawnPosition = GameManager.Instance.GetRandomSpawnPositionGround();

		Enemy boss = Instantiate(Bosses[levelIndex].enemyPrefab, bossSpawnPosition, Quaternion.identity).GetComponent<Enemy>();
		boss.InitializeEnemy(Bosses[levelIndex]);
		
		boss.InitializeBossHealthbar(CanvasManager.GetTab<GameplayTab>().GetBossHealth());

		boss.destroyedEvent.OnDestroyed += DestroyedEvent_OnBossDestroyed;
	}

	private void DestroyedEvent_OnBossDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		GameManager.Instance.DisableSpawners();

		ClearEnemies();

		GameManager.Instance.ChangeGameState(GameState.Evacuating);

		GameManager.Instance.SpawnEvacuationArea();
	}

	private void ClearEnemies()
	{
		ActiveEnemies.ForEach(x => x.GetComponent<Enemy>().animateEnemy.TurnOnRagdoll());
	}

	public void ScaleUpSpawns()
	{
		StartEnemyCount = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(Level + 1));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(Level + 1);
	}

	public void ScaleUpSpawns(float time)
	{
		StartEnemyCount = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(time));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(time);
	}

	public NavMeshTriangulation GetNavMeshTriangulation()
	{
		return navTriangulation;
	}
}
