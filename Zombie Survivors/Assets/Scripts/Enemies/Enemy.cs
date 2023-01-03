using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(Destroyed))]
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
	[HideInInspector] public EnemyDetailsSO enemyDetails;
	[HideInInspector] public EnemyController enemyController;
	[HideInInspector] public DealContactDamage dealContactDamage;
	[HideInInspector] public DestroyedEvent destroyedEvent;

	private HealthEvent healthEvent;
	private Health health;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
		destroyedEvent = GetComponent<DestroyedEvent>();
		dealContactDamage = GetComponent<DealContactDamage>();
		enemyController = GetComponent<EnemyController>();
		health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		//subscribe to health event
		healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
	}

	private void OnDisable()
	{
		//subscribe to health event
		healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
	}

	/// <summary>
	/// Handle health lost event
	/// </summary>
	private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		if (healthEventArgs.healthAmount <= 0)
		{
			destroyedEvent.CallDestroyedEvent(false, health.GetStartingHealth());
		}
	}

	/// <summary>
	/// Initialise the enemy
	/// </summary>
	public void EnemyInitialization(EnemyDetailsSO enemyDetails)
	{
		this.enemyDetails = enemyDetails;

		SetEnemyStartingHealth();
		SetMoveSpeed();
		SetContactDamage();
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
