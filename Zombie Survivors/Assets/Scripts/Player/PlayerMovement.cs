using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private FixedJoystick _joystick;
	[SerializeField] private float _moveSpeed = 5;

	[Tooltip("What should the distance between Player and closest enemy have to be, for Player to turn towards it/it/she/they/them")]
	[SerializeField] private float turnToEnemyDistance = 3f;
	[Tooltip("Turning speed")]
	[SerializeField] private float turnToEnemySpeed = 5f;

	public static List<Transform> enemiesTransforms = new List<Transform>();

	private Rigidbody _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		var target = GetClosestEnemy(enemiesTransforms);

		if ((target.position - transform.position).magnitude < turnToEnemyDistance)
		{
			transform.LookAt(target);
		}
	}

	private void FixedUpdate()
	{
		_rb.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rb.velocity.y, _joystick.Vertical * _moveSpeed);

		if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
		{
			transform.rotation = Quaternion.LookRotation(_rb.velocity);
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
