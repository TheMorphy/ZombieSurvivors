using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GapDirection
{
	North,
	South,
	East,
	West
}

[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
	public List<EnemyDetailsSO> BossDetails = new List<EnemyDetailsSO>();
	[Space]
	public List<EnemyDetailsSO> EnemiesDetails = new List<EnemyDetailsSO>();
	public ScalingConfigurationSO ScalingConfiguration;

	[Space(10)]
	[SerializeField] private int enemyCount = 15;
	[SerializeField] private float spawnDelay = 1.5f;
	[SerializeField] private float maxSpawnDistance = 10;
	[SerializeField] private float gapSizeDegrees = 45f; // gap size in degrees
	//[SerializeField] private float offsetGapAngle = 45f; // gap size in degrees

	private NavMeshTriangulation navTriangulation;
	public static List<Transform> ActiveEnemies;

	private int Level = 0;
	private List<EnemyDetailsSO> scaledEnemies = new List<EnemyDetailsSO>();

	private int enemiesAlive = 0;
	private int enemiesSpawned = 0;
	private int initialEnemiesToSpawn = 0;
	private float InitialSpawnDelay = 0;
	[SerializeField] private GapDirection gapDirection;


	[HideInInspector] public Player Player;

	private void Awake()
	{
		navTriangulation = NavMesh.CalculateTriangulation();
		initialEnemiesToSpawn = enemyCount;
		InitialSpawnDelay = spawnDelay;
	}

	private void Start()
	{
		ActiveEnemies = new List<Transform>();

		for (int i = 0; i < EnemiesDetails.Count; i++)
		{
			scaledEnemies.Add(EnemiesDetails[i].ScaleUpEnemies(ScalingConfiguration, 0));
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
		gapDirection = Utilities.GetRandomEnumValue<GapDirection>();

		for (int i = 0; i < EnemiesDetails.Count; i++)
		{
			scaledEnemies.Add(EnemiesDetails[i].ScaleUpEnemies(ScalingConfiguration, Level));
		}

		WaitForSeconds Wait = new WaitForSeconds(spawnDelay);
		while (enemiesSpawned < enemyCount)
		{
			Spawn();

			yield return Wait;
		}
	}

	private void Spawn()
	{
		float playerToCameraDistance = Vector3.Distance(Camera.main.transform.position, Player.transform.position);
		float halfCameraViewAngle = Mathf.Atan2(Camera.main.pixelHeight, Camera.main.pixelWidth) * Mathf.Rad2Deg / 2;
		float halfCameraViewSize = Mathf.Tan(halfCameraViewAngle * Mathf.Deg2Rad) * playerToCameraDistance;
		float spawnRadius = Mathf.Min(halfCameraViewSize * 1.5f, maxSpawnDistance);

		float gapAngle = 0f;
		switch (gapDirection)
		{
			case GapDirection.North:
				gapAngle = Mathf.PI / 2f;
				break;
			case GapDirection.West:
				gapAngle = Mathf.PI;
				break;
			case GapDirection.East:
				gapAngle = 0f;
				break;
			case GapDirection.South:
				gapAngle = Mathf.PI * 3f / 2f;
				break;
		}
		gapAngle -= (gapSizeDegrees * Mathf.Deg2Rad) / 2;

		gapAngle = (gapAngle + 2 * Mathf.PI) % (2 * Mathf.PI);

		float gapSize = gapSizeDegrees * Mathf.Deg2Rad;

		float angleStep = (Mathf.PI * 2) / enemyCount; // Calculate the angle between each enemy spawn point

		// Spawn enemies in a circle around the player with a gap in the specified direction
		for (int i = 0; i < enemyCount; i++)
		{
			float angle = angleStep * i;
			angle = (angle + 2 * Mathf.PI) % (2 * Mathf.PI); // Ensure angle is between 0 and 2*pi radians

			if (angle >= gapAngle && angle <= gapAngle + gapSize) // Check if angle is within gap
			{
				angle += gapSize; // Skip the gap area by shifting the angle by the gap size
			}

			Vector3 spawnPos = Player.transform.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0f, Mathf.Sin(angle) * spawnRadius);
			Vector3 screenPos = Camera.main.WorldToViewportPoint(spawnPos);

			// If the spawn point is inside the camera viewport, move it behind another enemy
			if (screenPos.x >= 0f && screenPos.x <= 1f && screenPos.y >= 0f && screenPos.y <= 1f)
			{
				Collider[] colliders = Physics.OverlapSphere(spawnPos, 5f);
				if (colliders.Length > 0)
				{
					// Find the closest enemy and move the spawn point behind it
					float minDist = Mathf.Infinity;
					Vector3 behindPos = Vector3.zero;
					foreach (Collider collider in colliders)
					{
						if (collider.CompareTag("Enemy"))
						{
							float dist = Vector3.Distance(spawnPos, collider.transform.position);
							if (dist < minDist)
							{
								minDist = dist;
								behindPos = collider.transform.position - (spawnPos - collider.transform.position).normalized * 2f;
							}
						}
					}

					spawnPos = behindPos;
				}
				else
				{
					// No other enemies in the area, use the original spawn point
					spawnPos = Player.transform.position + new Vector3(Mathf.Cos(angle) * (spawnRadius + 2f), 0f, Mathf.Sin(angle) * (spawnRadius + 2f));
				}
			}
			else
			{
				// Spawn point is outside camera viewport, use the original spawn point
				spawnPos = Player.transform.position + new Vector3(Mathf.Cos(angle) * spawnRadius, 0f, Mathf.Sin(angle) * spawnRadius);
			}
			GameObject enemyObj = Instantiate(scaledEnemies[0].enemyPrefab, spawnPos, Quaternion.identity);
			// Set the position and rotation of the spawned enemy
			enemyObj.transform.position = spawnPos;
			enemyObj.transform.rotation = Quaternion.identity;

			Enemy enemy = enemyObj.GetComponent<Enemy>();
			enemy.enemyController.GetAgent().enabled = false;

			enemy.InitializeEnemy(scaledEnemies[0]);

			enemy.destroyedEvent.OnDestroyed += DestroyedEvent_OnEnemyDestroyed;

			enemiesSpawned++;
			enemiesAlive++;
		}
	}

	private void DestroyedEvent_OnEnemyDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		if(destroyedEventArgs.playerDied == false)
		{
			enemiesAlive--;

			if (enemiesAlive == 0 && enemiesSpawned == enemyCount)
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

		Enemy boss = Instantiate(BossDetails[levelIndex].enemyPrefab, bossSpawnPosition, Quaternion.identity).GetComponent<Enemy>();
		boss.InitializeEnemy(BossDetails[levelIndex]);
		
		boss.InitializeBossHealthbar(CanvasManager.GetTab<GameplayTab>().GetBossHealth());

		boss.destroyedEvent.OnDestroyed += DestroyedEvent_OnBossDestroyed;
	}

	private void DestroyedEvent_OnBossDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
	{
		ClearEnemies();
		GameManager.Instance.ChangeGameState(GameState.Evacuating);
	}

	private void ClearEnemies()
	{
		ActiveEnemies.ForEach(x => x.GetComponent<Enemy>().animateEnemy.TurnOnRagdoll());
	}

	public void ScaleUpSpawns()
	{
		enemyCount = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(Level + 1));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(Level + 1);
	}

	public void ScaleUpSpawns(float time)
	{
		enemyCount = Mathf.FloorToInt(initialEnemiesToSpawn * ScalingConfiguration.SpawnCountCurve.Evaluate(time));
		spawnDelay = InitialSpawnDelay * ScalingConfiguration.SpawnRateCurve.Evaluate(time);
	}

	public NavMeshTriangulation GetNavMeshTriangulation()
	{
		return navTriangulation;
	}
}
