using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IdleEvent))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
	private Rigidbody rigidBody;
	private IdleEvent idleEvent;

	private void Awake()
	{
		// Load components
		rigidBody = GetComponent<Rigidbody>();
		idleEvent = GetComponent<IdleEvent>();
	}

	private void OnEnable()
	{
		// Subscribe to idle event
		idleEvent.OnIdle += IdleEvent_OnIdle;
	}

	private void OnDisable()
	{
		// Subscribe to idle event
		idleEvent.OnIdle -= IdleEvent_OnIdle;
	}

	private void IdleEvent_OnIdle(IdleEvent idleEvent)
	{
		MoveRigidBody();
	}

	/// <summary>
	/// Move the rigidbody component
	/// </summary>
	private void MoveRigidBody()
	{
		// ensure the rb collision detection is set to continuous
		rigidBody.velocity = Vector2.zero;
	}
}