using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ComradeMovement : MonoBehaviour
{
	private Comrade comrade;
    [SerializeField] private Transform bodyPivot;

	private FireWeapon fireWeapon;

	private float turnSpeed = 720;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();
		fireWeapon = GetComponent<FireWeapon>();
	}

	private void Update()
	{
		HandleRotations();

		fireWeapon.FireWeapn();
	}

	private void HandleRotations()
	{
		var target = GetClosestEnemy(EnemySpawner.activeEnemies);

		if (target != null)
		{
			Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
			bodyPivot.rotation = Quaternion.RotateTowards(bodyPivot.rotation, rotation, turnSpeed * Time.deltaTime);
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
