using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	private CinemachineTransposer transposer;

	[SerializeField] private float cameraZoomSpeed = 2f;
	[SerializeField] private float maxScreenWidth = 0.8f;
	[SerializeField] private float minCameraDistance = 10f;

	private Transform target = null;
	private Vector3 directionVector;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
	}

	private void LateUpdate()
	{
		if (target == null)
			return;

		//float childObjectsWidth = GetChildObjectsWidth(target);
		//directionVector = (target.position - virtualCamera.transform.position).normalized;
		//if (childObjectsWidth > maxScreenWidth)
		//{
		//	float cameraDistance = Mathf.Lerp(transposer.m_FollowOffset.magnitude,
		//		Mathf.Max(childObjectsWidth / (1 - maxScreenWidth * 2), minCameraDistance), cameraZoomSpeed * Time.deltaTime);
		//	transposer.m_FollowOffset = -directionVector * cameraDistance;
		//}
		//else
		//{
		//	float cameraDistance = Mathf.Lerp(transposer.m_FollowOffset.magnitude, minCameraDistance, cameraZoomSpeed * Time.deltaTime);
		//	transposer.m_FollowOffset = -directionVector * cameraDistance;
		//}
	}

	private float GetChildObjectsWidth(Transform target)
	{
		Bounds bounds = new Bounds(target.position, Vector3.zero);
		for (int i = 1; i < target.childCount; i++)
		{
			bounds.Encapsulate(target.GetChild(i).GetComponent<Collider>().bounds);
		}
		Vector3 viewportMin = Camera.main.WorldToViewportPoint(bounds.min);
		Vector3 viewportMax = Camera.main.WorldToViewportPoint(bounds.max);
		float viewportWidth = Mathf.Abs(viewportMax.x - viewportMin.x);
		return viewportWidth;
	}

	public void SetInitialTarget(Transform target)
	{
		virtualCamera.LookAt = target;
		virtualCamera.Follow = target;
		this.target = target;
	}
}

