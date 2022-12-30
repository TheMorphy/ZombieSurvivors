using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
	private NavMeshAgent _agent;

	private Transform _player;

	private void Update()
	{
		if(_agent.enabled)
		{
			_agent.destination = _player.position;
			transform.LookAt(_player);
		}
	}

	private void OnDisable()
	{
		_agent.enabled = false;
	}
	private void OnEnable()
	{
		_agent = GetComponent<NavMeshAgent>();
		_player = GameObject.FindGameObjectWithTag("Player").transform;

		_agent.enabled = true;
	}

	public NavMeshAgent GetAgent()
	{
		return _agent;
	}
}
