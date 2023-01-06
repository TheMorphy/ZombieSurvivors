using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
	private NavMeshAgent _agent;
	private Enemy enemy;

	private Transform _player;
	private Vector3 startPos;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
		enemy = GetComponent<Enemy>();

		_player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Start()
	{
		startPos = transform.position;
	}

	private void Update()
	{
		if(_agent.enabled)
		{
			if(_player != null)
			{
				_agent.destination = _player.position;
				transform.LookAt(_player);
			}
			else
			{
				_agent.isStopped = true;

				_agent.destination = startPos;
			}
		}
	}
	
	public NavMeshAgent GetAgent()
	{
		return _agent;
	}
}
