using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MultiplicationAreaPointer : MonoBehaviour
{
	[SerializeField] private Camera uiCamera;
	[SerializeField] private Sprite arrowSprite;

	private List<TargetPointer> targetPointers;

	private void OnEnable()
	{
		StaticEvents.OnCircleSpawned += StaticEvents_OnCircleSpawned;
		StaticEvents.OnCircleDespawned += StaticEvents_OnCircleDespawned;
	}

	private void OnDisable()
	{
		StaticEvents.OnCircleSpawned -= StaticEvents_OnCircleSpawned;
		StaticEvents.OnCircleDespawned -= StaticEvents_OnCircleDespawned;
	}

	private void Awake()
	{
		targetPointers = new List<TargetPointer>();
	}

	private void Update()
	{
		foreach (var pointer in targetPointers)
		{
			pointer.Update();
		}
	}

	public TargetPointer CreatePointer(Vector3 targetPosition)
	{
		GameObject pointerGameObject = Instantiate(transform.Find("Pointer").gameObject);
		pointerGameObject.transform.SetParent(transform, false);
		pointerGameObject.SetActive(true);
		TargetPointer targetPointer = new TargetPointer(targetPosition, pointerGameObject, arrowSprite, uiCamera);
		targetPointers.Add(targetPointer);

		return targetPointer;
	}

	private void StaticEvents_OnCircleSpawned(CircleSpawnedEventArgs circleSpawnedEventArgs)
	{
		CreatePointer(circleSpawnedEventArgs.spawnPosition);
	}

	private void StaticEvents_OnCircleDespawned(CircleDespawnedEventArgs circleDespawnedEventArgs)
	{
		targetPointers.First(x => x.targetPosition == circleDespawnedEventArgs.despawnPosition).DestroySelf();
	}

	#region Pointer Object

	public class TargetPointer
	{
		private Camera uiCamera;
		public Vector3 targetPosition;
		private Sprite arrowSprite;
		private RectTransform pointerTransform;
		private GameObject pointerGameObject;
		private Image pointerImage;
		float borderSzie = 150f;

		public TargetPointer(Vector3 targetPosition, GameObject pointerGameObject, Sprite arrowSprite, Camera uiCamera)
		{
			this.targetPosition = targetPosition;
			this.pointerGameObject = pointerGameObject;
			this.arrowSprite = arrowSprite;
			this.uiCamera = uiCamera;

			pointerTransform = pointerGameObject.GetComponent<RectTransform>();
			pointerImage = pointerTransform.gameObject.GetComponent<Image>();
		}

		public void Update()
		{
			if (pointerGameObject == null)
				return;

			Vector3 toPosition = targetPosition;
			Vector3 fromPosition = Camera.main.transform.position;

			Vector3 dir = (toPosition - fromPosition).normalized;

			float angle = Utilities.GetAngleFromVectorFloatXZ(dir);

			pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

			Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);

			bool isOffScreen =
				targetPositionScreenPoint.x <= borderSzie || targetPositionScreenPoint.x >= Screen.width - borderSzie ||
				targetPositionScreenPoint.y <= borderSzie || targetPositionScreenPoint.y >= Screen.height - borderSzie;

			if (isOffScreen)
			{
				pointerImage.enabled = true;
				pointerImage.sprite = arrowSprite;
				Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

				cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, borderSzie, Screen.width - borderSzie);
				cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, borderSzie, Screen.height - borderSzie);

				Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
				pointerTransform.position = pointerWorldPosition;
				pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0);

			}
			else
			{
				pointerImage.enabled = false;
				Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
				pointerTransform.position = pointerWorldPosition;
				pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0);
			}
		}

		public void DestroySelf()
		{
			Destroy(pointerGameObject);
		}
	}
	#endregion
}
