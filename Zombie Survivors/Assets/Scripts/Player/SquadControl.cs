using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SquadControl : MonoBehaviour
{
	[SerializeField] private GameObject comradePrefab;

	[Range(0f, 1f)]
	[SerializeField] private float DistanceFactor, Radius;

	[SerializeField] private int squadAmmount;

	public event Action<SquadControl, SquadControlEventArgs> OnSquadAmmountChanged;

	public static List<Transform> ComradesTransforms = new List<Transform>();

	private void Start()
	{
		squadAmmount = transform.childCount - 1;

		GameManager.Instance.AddTargetToCamera(transform, 1, 1);
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
			GameObject member = Instantiate(comradePrefab, transform.position, Quaternion.identity, transform);

			GameManager.Instance.AddTargetToCamera(member.transform, 1, 1);
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

		FormatSquad();
	}

	public void CallSquadChangedEvent(int squadSize)
	{
		OnSquadAmmountChanged?.Invoke(this, new SquadControlEventArgs()
		{
			squadSize = squadSize
		});
	}
}

public class SquadControlEventArgs : EventArgs
{
	public int squadSize;
}
