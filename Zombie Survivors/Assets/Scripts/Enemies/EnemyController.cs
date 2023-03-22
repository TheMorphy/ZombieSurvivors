using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
	private NavMeshAgent agent;
	private Enemy enemy;
	[SerializeField] private float maxTime = 1.0f;
	[SerializeField] private float maxDistance = 1.0f;
	private float timer = 0;
	
	private Vector3 startPos;
	private Transform target;
	[HideInInspector] public bool ExpDropped = false;
	[HideInInspector] public bool IsAttacking = false;

	[HideInInspector] public List<Collider> HitColliders;
	[HideInInspector] public List<DealContactDamage> DamageDealers;
	[HideInInspector] public Limb LimbsHit;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();
		agent = GetComponent<NavMeshAgent>();

		DamageDealers = GetComponentsInChildren<DealContactDamage>().Skip(1).ToList();
		DamageDealers.ForEach(x => HitColliders.Add(x.GetComponent<Collider>()));
	}

	private void Start()
	{
		SetContactDamage();
		DisableHitboxes();

		agent.enabled = true;
		agent.stoppingDistance = maxDistance;
		startPos = transform.position;
	}

	private void Update()
	{
		if(agent.enabled)
		{
			if (SquadControl.Comrades.Count > 0)
			{
				target = GetClosestComrade(SquadControl.Comrades);

				if (IsAttacking == false)
				{
					timer -= Time.deltaTime;
					if(timer < 0.0f)
					{
						float sqDistance = (target.position - transform.position).sqrMagnitude;

						if(sqDistance > maxDistance * maxDistance)
						{
							agent.destination = target.position;
							timer = maxTime;
						}
						else
						{
							IsAttacking = true;
							enemy.animateEnemy.StartAttacking();
						}	
					}
				}
				
				transform.LookAt(target);
			}
			else
			{
				agent.destination = startPos;
			}
		}
	}

	public void DisableAttacking()
	{
		DisableHitboxes();
		IsAttacking = false;
		enemy.animateEnemy.StopAttacking();
	}

	public void DropEXP()
	{
		if (enemy.enemyDetails.Class == EnemyClass.Boss)
			return;

		GameObject exp = Instantiate(GameResources.Instance.ExpDrop, transform.position, Quaternion.identity);
		exp.GetComponent<ExpDrop>().SetExpValue(enemy.enemyDetails.EXP_Increase);
		ExpDropped = true;
		enemy.destroyedEvent.CallDestroyedEvent(false, enemy.enemyDetails.EXP_Increase);
	}

	public void DisableEnemy()
	{
		agent.enabled = false;

		enabled = false;
	}

	Transform GetClosestComrade(List<Comrade> comrades)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;

		int index = 0;
		if (comrades.Count > 1)
			index = 1;
		
		for (int i = index; i < comrades.Count; i++)
		{
			Vector3 directionToTarget = comrades[i].transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = comrades[i].transform;
			}
		}

		return bestTarget;
	}

	public NavMeshAgent GetAgent()
	{
		return agent;
	}

	public float GetMoveSpeed()
	{
		return agent.velocity.magnitude;
	}

	private void SetContactDamage()
	{
		DamageDealers.ForEach(x => x.SetContactDamage(enemy.enemyDetails.Damage));
		
	}
	public void EnableHitboxes()
	{
		HitColliders.ForEach(x => x.enabled = true);
	}

	public void DisableHitboxes()
	{
		HitColliders.ForEach(x => x.enabled = false);
	}
}
