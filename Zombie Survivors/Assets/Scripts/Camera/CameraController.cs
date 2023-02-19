using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	[SerializeField] 
	CinemachineVirtualCamera virtualCamera;

	// Should we remove these?
	[SerializeField] float cameraZoomSpeed = 2f;
	[SerializeField] float maxScreenWidth = 0.8f;
	[SerializeField] float minCameraDistance = 10f;

	[SerializeField, Range(0.01f, 1f), Tooltip("What percentage of the screen should the squad take up?")]
	float targetSquadFootprint = 0.1f;

	// Previous fields.
	Transform target = null;
	Vector3 directionVector;
	CinemachineTransposer transposer;

	// New fields.
	Camera _camera;
	SquadControl _squad;
	Vector3 _initialOffset;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		// Cache a reference to the transposer.
		transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
		if(!transposer) Debug.LogWarning("The camera controller could not find the Cinemachine transposer.!");
		
		// Cache a reference to camera main.
		_camera = Camera.main;
		
		// Cache the initial desired camera offset.
		_initialOffset = transposer.m_FollowOffset;
		
		// NOTE: I moved to set the reference to squadControl, after the upgrades have been set up: Player -> line 145

		// Temporary method to get access to the squad controller. We might want to make this more robust.
		//_squad = FindObjectOfType<SquadControl>();
		//if(!_squad) Debug.LogWarning("The camera controller could not find the squad!");
	}

	void LateUpdate()
	{
		// Ensure squad was found, resize the camera to the squad.
		if (_squad != null)
		{
			ResizeCameraToSquad();
		}
		
	}

	/// <summary>
	/// Repositions the transposer follow offset to the squad size given a target screen percentage.
	/// </summary>
	void ResizeCameraToSquad()
	{
		// Get the world position of the furthest member.
		Vector3 furthestMember = _squad.FurthestMemberPosition();

		// Ensure the minimum width, this is used when there is no distance because there is only one member.
		float squadWidth = Mathf.Max(Vector3.Distance(_squad.transform.position, furthestMember), 2.5f);
		
		// Calculate the desired screen with in world units.
		float desiredWidth = squadWidth * (1f / targetSquadFootprint);

		// Normalize the initial offset vector to get the direction.
		// NOTE: I see direction is already in this script, this could likely be removed and should be calculated on start.
		Vector3 direction = _initialOffset.normalized;
		
		// Figure out the distance the camera needs to be from the follow target from the width.
		float distance = DistanceRequiredToGetDesiredFrustumWidth(desiredWidth);
		
		// Update the follow offset.
		transposer.m_FollowOffset = direction * distance;
	}

	/// <summary>
	/// Calculates a distance from the camera that is required to achieve the desired width.
	/// </summary>
	/// <param name="frustumWidth">Desired camera frustum width in world units.</param>
	/// <returns>Distance from the camera to achieve target width.</returns>
	float DistanceRequiredToGetDesiredFrustumWidth(float frustumWidth)
	{
		// Calculate the frustum height given the width and aspect.
		float frustumHeight = frustumWidth / _camera.aspect;
		
		// Calculate the distance the camera needs to be to achieve the target height.
		// NOTE: More about this can be read here: https://docs.unity3d.com/Manual/FrustumSizeAtDistance.html
		return frustumHeight * 0.5f / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
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
		if (target.TryGetComponent(out SquadControl squadControl))
			_squad = squadControl;

		virtualCamera.LookAt = target;
		virtualCamera.Follow = target;
		this.target = target;
	}
}

