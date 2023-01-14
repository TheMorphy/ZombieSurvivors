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

	public event Action<SquadControl, int> OnSquadIncrease;

	public static List<Transform> ComradesTransforms = new List<Transform>();

	private void Start()
	{
		squadAmmount = transform.childCount - 1;
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

	private void ApplyMultiplication()
	{
		for (int i = 1; i < transform.childCount; i++)
		{
			var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
			var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

			var newPos = new Vector3(x, transform.position.y, z);

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

		ApplyMultiplication();

		OnSquadIncrease?.Invoke(this, squadAmmount);
	}

	public int GetSquadAmmount()
	{
		return squadAmmount;
	}
}
