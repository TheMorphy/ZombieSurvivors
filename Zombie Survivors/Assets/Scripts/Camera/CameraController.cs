using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform cameraFollowTarget;

	private static CinemachineTargetGroup targetGroup;

	private Transform target;

	private void Awake()
	{
		targetGroup = cameraFollowTarget.GetComponent<CinemachineTargetGroup>();
	}

	private void LateUpdate()
	{
		if (target == null)
			return;

		cameraFollowTarget.position = target.position;
	}

	public static void AddToTargetGroup(Transform target)
	{
		targetGroup.AddMember(target, 1, 7);
	}

	public static void IncreaseTargetGroupBoundingBox(float increaseAmmount)
	{
		targetGroup.m_Targets[0].radius = increaseAmmount > 7 ? increaseAmmount : 7;
	}

	public static void RemoveFromTargetGroup(Transform transform)
	{
		targetGroup.RemoveMember(transform);

		targetGroup.m_Targets[0].radius -= 1;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	//[SerializeField] private Vector3 offset;
	//[SerializeField] private float zoomSpeed = 5f;
	//[SerializeField] private float smoothTime = 0.1f;
	//[SerializeField] private float margin = 0.15f;
	//[SerializeField] private float minDistance = 15f;
	//private Vector3 velocity;

	//[HideInInspector] public Player Player;

	//private Vector3? targetPosition = null;

	//private void LateUpdate()
	//{
	//	if (Player == null)
	//	{
	//		return;
	//	}

	//	Bounds childrenBounds = Player.squadControl.GetChildrenBounds();
	//	Vector3 boundingBoxMin = Camera.main.WorldToScreenPoint(childrenBounds.min);
	//	Vector3 boundingBoxMax = Camera.main.WorldToScreenPoint(childrenBounds.max);
	//	float boundingWidth = boundingBoxMax.x - boundingBoxMin.x;
	//	float screenWidth = Screen.width * (1 - margin * 2);

	//	float occupiedWidth = boundingWidth / screenWidth;

	//	var targetPosition = Player.transform.position + offset;

	//	// Zoom in
	//	if (occupiedWidth > 1)
	//	{
	//		targetPosition += (occupiedWidth - 1) * zoomSpeed * (transform.position - Player.transform.position).normalized;
	//	}
	//	// Zoom out
	//	else if (occupiedWidth < 1)
	//	{
	//		targetPosition -= (1 - occupiedWidth) * zoomSpeed * (transform.position - Player.transform.position).normalized;
	//	}

	//	transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

	//	float distance = Vector3.Distance(transform.position, Player.transform.position);
	//	if (distance < minDistance)
	//	{
	//		offset += (minDistance - distance) * (transform.position - Player.transform.position).normalized;
	//	}
	//}

	//public void SetNewTargetPosition(Vector3 targetPosition)
	//{
	//	Player = null;

	//	this.targetPosition = targetPosition;
	//}
}
