using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Player player;

	[SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 rotation;
	[SerializeField] private int zoomThreshhold = 10;
	[SerializeField] private float zoomAmmount = 2;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void LateUpdate()
	{
		transform.position = player.transform.position + offset;
	}

	public void ZoomOut()
	{
		if (player.transform.childCount - 1 >= zoomThreshhold)
		{
			Vector3 zoomDirection = (transform.position - player.transform.position).normalized;

			transform.localPosition += zoomDirection * zoomAmmount;
		}
	}
}
