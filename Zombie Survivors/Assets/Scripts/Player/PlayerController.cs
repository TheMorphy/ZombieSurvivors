using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private FixedJoystick _joystick;

	[SerializeField] private float _moveSpeed;

	private void FixedUpdate()
	{
		_rb.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rb.velocity.y, _joystick.Vertical * _moveSpeed);

		if(_joystick.Horizontal != 0 || _joystick.Vertical != 0)
		{
			transform.rotation = Quaternion.LookRotation(_rb.velocity);
		}
	}
}
