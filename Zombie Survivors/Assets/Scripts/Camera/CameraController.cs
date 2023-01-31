using Cinemachine;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform cameraFollowTarget;
	[SerializeField] private const float minimumRadius = 7;

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

	public static void UpdateTargetGroupBoundingBox(float increaseAmmount)
	{
		targetGroup.m_Targets[0].radius = increaseAmmount > minimumRadius ? increaseAmmount : minimumRadius;
	}

	public void SetNewTarget(Transform target)
	{
		targetGroup.m_Targets.ToList().RemoveAll(x => x.target);

		targetGroup.AddMember(target, 1, 7);
		this.target = target;
	}
}
