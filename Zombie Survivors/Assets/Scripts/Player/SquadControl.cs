using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SquadControl : MonoBehaviour
{
	[SerializeField] private GameObject comradePrefab;
	private Player player;

	[Range(0f, 1f)]
	[SerializeField] private float DistanceFactor, Radius;

	[SerializeField] private int squadAmmount;

	public event Action<SquadControl, int> OnSquadIncrease;

	public static List<Transform> ComradesTransforms = new List<Transform>();

	private void Awake()
	{
		player = GetComponentInChildren<Player>();
	}

	private void Start()
	{
		squadAmmount = transform.childCount;
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
		for (int i = 0; i < transform.childCount; i++)
		{
			var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
			var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

			var newPos = new Vector3(x, transform.position.y, z);

			transform.transform.GetChild(i).DOLocalMove(newPos, 0.7f).SetEase(Ease.OutBack);
		}
	}

	public void CreateComrades(int number)
	{
		for (int i = squadAmmount; i < number; i++)
		{
			GameObject comObj = Instantiate(comradePrefab, transform.position, Quaternion.identity, transform);
			comObj.GetComponent<Comrade>().SetPlayerReference(player);

			ComradesTransforms.Add(comObj.transform);
		}

		squadAmmount = transform.childCount;

		ApplyMultiplication();

		OnSquadIncrease?.Invoke(this, squadAmmount);
	}

	public int GetSquadAmmount()
	{
		return squadAmmount;
	}
}
