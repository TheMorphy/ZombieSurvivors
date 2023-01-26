using DG.Tweening.Core.Easing;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Vector3 offset;
	[SerializeField] private Vector3 rotationOffset;
	[SerializeField] private float zoomOutSpeed = 5f;
	[SerializeField] private float zoomInSpeed = 1f;
	[SerializeField] private float smoothTime = 0;
	[SerializeField] private float margin = 0.15f;
	[SerializeField] private float minDistance = 15f;
	private Vector3 velocity = Vector3.zero;

	[HideInInspector] public Player Player;

	private Vector3 targetPosition;

	private void Start()
	{
		transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
	}

	private void LateUpdate()
	{
		if (Player != null)
		{
			FollowPlayer();
		}
		else
		{
			FollowTarget();
		}
	}

	private void FollowTarget()
	{
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition + offset, ref velocity, 1.2f);
	}
	
	private void FollowPlayer()
	{
		Bounds childrenBounds = Player.squadControl.GetChildrenBounds();
		Vector3 boundingBoxMin = Camera.main.WorldToScreenPoint(childrenBounds.min);
		Vector3 boundingBoxMax = Camera.main.WorldToScreenPoint(childrenBounds.max);
		float boundingWidth = boundingBoxMax.x - boundingBoxMin.x;
		float screenWidth = Screen.width * (1 - margin * 2);

		float occupiedWidth = boundingWidth / screenWidth;

		var targetPosition = Player.transform.position + offset;

		// Zoom in
		if (occupiedWidth > 1)
		{
			targetPosition += (occupiedWidth - 1) * zoomInSpeed * (transform.position - Player.transform.position).normalized;
		}
		// Zoom out
		else if (occupiedWidth < 1)
		{
			targetPosition -= (1 - occupiedWidth) * zoomOutSpeed * (transform.position - Player.transform.position).normalized;
		}
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		float distance = Vector3.Distance(transform.position, Player.transform.position);
		if (distance < minDistance)
		{
			offset += (minDistance - distance) * (transform.position - Player.transform.position).normalized;
		}
	}

	public void SetNewTargetPosition(Vector3 targetPosition)
	{
		Player = null;

		this.targetPosition = targetPosition;
	}
}