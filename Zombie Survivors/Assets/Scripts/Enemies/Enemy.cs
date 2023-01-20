using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(AnimateEnemy))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]

public class Enemy : MonoBehaviour
{
	[HideInInspector] public EnemyDetailsSO enemyDetails;
	[HideInInspector] public EnemyController enemyController;
	[HideInInspector] public Destroyed destroyed;
	[HideInInspector] public DestroyedEvent destroyedEvent;
	[HideInInspector] public DealContactDamage dealContactDamage;
	[HideInInspector] public AnimateEnemy animateEnemy;
	[HideInInspector] public Animator animator;

	private HealthEvent healthEvent;
	private Health health;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
		destroyed = GetComponent<Destroyed>();
		destroyedEvent = GetComponent<DestroyedEvent>();
		dealContactDamage = GetComponent<DealContactDamage>();
		enemyController = GetComponent<EnemyController>();
		animateEnemy = GetComponent<AnimateEnemy>();
		health = GetComponent<Health>();
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		EnemySpawner.activeEnemies.Add(transform);

		//subscribe to health event
		healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
	}

	private void OnDisable()
	{
		healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
	}

	private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		health.UpdateHealthBar(healthEventArgs.healthPercent);
		StartCoroutine(health.ShowHealthBarForSeconds(1f));

		if (healthEventArgs.healthAmount <= 0)
		{
			EnemySpawner.activeEnemies.Remove(transform);

			enemyController.GetAgent().isStopped = true;

			animator.SetBool("Die", true);
			//destroyedEvent.CallDestroyedEvent(false, health.GetStartingHealth());
		}
	}

	/// <summary>
	/// Initialise the enemy
	/// </summary>
	public void InitializeEnemy(EnemyDetailsSO enemyDetails)
	{
		this.enemyDetails = enemyDetails;

		SetEnemyStartingHealth();
		SetMoveSpeed();
		SetContactDamage();
	}

	public void IncreaseEnemyHealth()
	{

	}

	/// <summary>
	/// Set the starting health for the enemy
	/// </summary>
	private void SetEnemyStartingHealth()
	{
		health.SetStartingHealth(enemyDetails.Health);
	}

	private void SetContactDamage()
	{
		dealContactDamage.SetContactDamage(enemyDetails.Damage);
	}
	private void SetMoveSpeed()
	{
		enemyController.GetAgent().speed = enemyDetails.MoveSpeed;
	}
}
