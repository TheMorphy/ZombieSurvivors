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
	private Vector3 startPos;
	private Transform target;

	[HideInInspector] public bool ExpDropped = false;
	public bool Attacking;

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

		startPos = transform.position;
	}

	private void Update()
	{
		if(agent.enabled)
		{
			if (SquadControl.ComradesTransforms.Count > 0)
			{
				target = GetClosestComrade(SquadControl.ComradesTransforms);

				if (Attacking == false)
				{
					agent.destination = target.position;
				}
				
				transform.LookAt(target);
			}
			else
			{
				agent.destination = startPos;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Comrade"))
		{
			Attacking = true;
			StartCoroutine(enemy.animateEnemy.Attack());
		}
	}

	public void DisableAttacking()
	{
		DisableHitboxes();
		Attacking = false;
		StopCoroutine(enemy.animateEnemy.Attack());
	}

	public void DropEXP()
	{
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

	Transform GetClosestComrade(List<Transform> comrades)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;

		int index = 0;
		if (comrades.Count > 1)
			index = 1;
		
		for (int i = index; i < comrades.Count; i++)
		{
			Vector3 directionToTarget = comrades[i].position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = comrades[i];
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
