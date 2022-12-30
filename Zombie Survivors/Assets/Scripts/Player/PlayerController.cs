using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static List<Transform> enemiesTransforms = new List<Transform>();

	private void Update()
	{
		GetClosestEnemy(enemiesTransforms);
	}

	Transform GetClosestEnemy(List<Transform> enemies)
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		for (int i = 0; i < enemies.Count; i++)
		{
			Vector3 directionToTarget = enemies[i].position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = enemies[i];
			}
		}

		return bestTarget;
	}
}
