using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(AnimateEnemy))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]

public class Enemy : MonoBehaviour
{
	[HideInInspector] public EnemyDetailsSO enemyDetails;
	[HideInInspector] public EnemyController enemyController;
	[HideInInspector] public DealContactDamage dealContactDamage;
	[HideInInspector] public AnimateEnemy animateEnemy;
	[HideInInspector] public Animator animator;

	private HealthEvent healthEvent;
	private Health health;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
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
		healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
	}

	private void OnDisable()
	{
		EnemySpawner.activeEnemies.Remove(transform);

		healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
	}

	/// <summary>
	/// Handle health lost event
	/// </summary>
	private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		if (healthEventArgs.healthAmount <= 0)
		{
			enemyController.GetAgent().isStopped = true;

			animator.SetTrigger("Die");
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
