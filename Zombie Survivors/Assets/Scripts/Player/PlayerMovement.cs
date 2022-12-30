using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private FixedJoystick _joystick;
	[SerializeField] private float _moveSpeed = 5;

	private Rigidbody _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		_rb.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rb.velocity.y, _joystick.Vertical * _moveSpeed);

		if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
		{
			transform.rotation = Quaternion.LookRotation(_rb.velocity);
		}
	}
}
