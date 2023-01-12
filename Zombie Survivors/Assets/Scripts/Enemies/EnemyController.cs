using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
	private NavMeshAgent _agent;

	private Vector3 startPos;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		startPos = transform.position;
	}

	private void Update()
	{
		if(_agent.enabled)
		{
			if(SquadControl.ComradesTransforms.Count != 0)
			{
				var target = GetClosestComrade(SquadControl.ComradesTransforms);

				_agent.destination = target.position;
				transform.LookAt(target);
			}
			else
			{
				_agent.isStopped = true;

				_agent.destination = startPos;
			}
		}
	}

	Transform GetClosestComrade(List<Transform> comrades)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		for (int i = 0; i < comrades.Count; i++)
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
		return _agent;
	}
}
