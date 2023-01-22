using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SquadControl : MonoBehaviour
{
	[SerializeField] private GameObject comradePrefab;

	[Range(0f, 1f)]
	[SerializeField] private float DistanceFactor, Radius;

	[SerializeField] private int squadAmmount;

	public event Action<SquadControl, SquadControlEventArgs> OnSquadAmmountChanged;

	public static List<Transform> ComradesTransforms = new List<Transform>();

	BoxCollider collider;
	float timer;

	private void Awake()
	{
		collider = GetComponent<BoxCollider>();
	}

	private void Start()
	{
		squadAmmount = transform.childCount - 1;
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if(timer > 2f)
		{
			UpdateColliderSize();
			timer = 0;
		}
	}

	public void IncreaseSquadSize(int randomValue, bool multiply)
	{
		if (multiply == false)
		{
			CreateComrades(randomValue + squadAmmount);
		}
		else
		{
			CreateComrades(randomValue * squadAmmount);

		}
	}

	public Bounds GetChildrenBounds()
	{
		Bounds bounds = new Bounds(transform.GetChild(0).position, Vector3.zero);

		for (int i = 1; i < transform.childCount; i++)
		{
			bounds.Encapsulate(transform.GetChild(i).GetComponent<Collider>().bounds);
		}
		return bounds;
	}

	private void UpdateColliderSize()
	{
		Bounds childrenBounds = GetChildrenBounds();
		Vector3 boundingBoxMin = childrenBounds.min;
		Vector3 boundingBoxMax = childrenBounds.max;
		float boundingWidthX = (boundingBoxMax.x - boundingBoxMin.x);
		float boundingWidthZ = (boundingBoxMax.z - boundingBoxMin.z);

		collider.size = new Vector3(boundingWidthX, collider.size.y, boundingWidthZ);
	}

	public void FormatSquad()
	{
		for (int i = 1; i < transform.childCount; i++)
		{
			var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
			var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

			var newPos = new Vector3(x, 0f, z);

			transform.GetChild(i).DOLocalMove(newPos, 0.7f).SetEase(Ease.OutBack);
		}
	}

	public void CreateComrades(int number)
	{
		for (int i = squadAmmount; i < number; i++)
		{
			Instantiate(comradePrefab, transform.position, Quaternion.identity, transform);
		}
		
		squadAmmount = transform.childCount - 1;

		FormatSquad();

		CallSquadChangedEvent(squadAmmount);
	}

	public int GetSquadAmmount()
	{
		return squadAmmount;
	}

	public void RemoveFromSquad(Transform comradeTransform)
	{
		comradeTransform.parent = null;

		squadAmmount--;
		
		ComradesTransforms.Remove(comradeTransform);
		CallSquadChangedEvent(squadAmmount);
	}

	public void CallSquadChangedEvent(int squadSize)
	{
		OnSquadAmmountChanged?.Invoke(this, new SquadControlEventArgs()
		{
			squadSize = squadSize,
			previousSquadSize = squadAmmount
		});
	}
}

public class SquadControlEventArgs : EventArgs
{
	public int squadSize;
	public int previousSquadSize;
}
