using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ComradeMovement : MonoBehaviour
{
    [SerializeField] private Transform bodyPivot;

	private Comrade comrade;
	private float turnSpeed = 720;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();
	}

	private void Update()
	{
		HandleRotations();
	}

	private void HandleRotations()
	{
		var target = GetClosestEnemy(EnemySpawner.ActiveEnemies);

		if (target != null)
		{
			Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
			bodyPivot.rotation = Quaternion.RotateTowards(bodyPivot.rotation, rotation, turnSpeed * Time.deltaTime);
		}
		else
		{
			
			Vector3 moveDirection = comrade.Player.PlayerController.GetMoveDirection();
			if (moveDirection.magnitude > 0)
			{
				Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
				bodyPivot.rotation = Quaternion.RotateTowards(bodyPivot.rotation, targetRotation, turnSpeed * Time.deltaTime);
			}
		}
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
