using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavMeshAgent : MonoBehaviour
{
    private NavMeshAgent agent;

	public bool velocity;
	public bool desiredVelocity;
	public bool path;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void OnDrawGizmos()
	{
		if (velocity)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
		}
		
		if (desiredVelocity)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
		}
		
		if (path)
		{
			var path = agent.path;
			Vector3 previousCorner = transform.position;
			Gizmos.color = Color.red;
			foreach (var corner in path.corners)
			{
				Gizmos.DrawLine(previousCorner, corner);
				Gizmos.DrawSphere(corner, 0.1f);
				previousCorner = corner;
			}
		}


	}
}
