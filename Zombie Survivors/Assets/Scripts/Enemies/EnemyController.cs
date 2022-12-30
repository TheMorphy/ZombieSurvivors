using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
	private NavMeshAgent _agent;

	private Transform _player;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		_player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		_agent.destination = _player.position;
		transform.LookAt(_player);
	}

	private void OnDisable()
	{
		_agent.enabled = false;
	}

	public NavMeshAgent GetAgent()
	{
		return _agent;
	}

	public void EnableNavMeshAgent()
	{
		_agent.enabled = true;
	}
}
