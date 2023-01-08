using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
	private Player player;
	[SerializeField] private float deathTime = 1f;
	[SerializeField] private float preDeathWait = 3f;

	private void Awake()
	{
		player = GetComponent<Player>();
	}

	public void PlayerDeath()
	{
		player.playerController.DisablePlayerMovement();

		Destroy(gameObject);
	}
}
