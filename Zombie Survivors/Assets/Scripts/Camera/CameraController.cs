using Cinemachine;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	[SerializeField] private Transform cameraFollowTarget;
	[SerializeField] private float minimumRadius = 7;
	[SerializeField] private float zoomSpeed = 5;

	private static CinemachineTargetGroup targetGroup;
	private Transform target;

	private float counter = 0;
	private float currentRadius = 0;

	private void Awake()
	{
		Instance = this;

		targetGroup = cameraFollowTarget.GetComponent<CinemachineTargetGroup>();
	}

	private void LateUpdate()
	{
		if (target == null)
			return;

		cameraFollowTarget.position = target.position;
		currentRadius = targetGroup.m_Targets[0].radius;
	}

	public void UpdateTargetGroupBoundingBox(float increaseAmmount)
	{
		counter += Time.deltaTime * zoomSpeed;

		if (counter <= increaseAmmount)
		{
			var percentageIncrease = counter / increaseAmmount;
			var newRadius = Mathf.Min(targetGroup.m_Targets[0].radius * (1.0f + percentageIncrease), increaseAmmount);

			// zoom in if new radius is smaller, otherwise zoom out
			if (newRadius < currentRadius)
			{
				newRadius = Mathf.Max(newRadius, minimumRadius);
			}
			else
			{
				newRadius = Mathf.Min(newRadius, increaseAmmount);
			}

			// set minimum radius
			targetGroup.m_Targets[0].radius = Mathf.Max(newRadius, minimumRadius);
		}
		else
		{
			counter = 0;
		}
	}

	public void SetNewTarget(Transform target)
	{
		//targetGroup.m_Targets.ToList().RemoveAll(x => x.target);

		targetGroup.AddMember(target, 1, 1);
		this.target = target;
	}
}
