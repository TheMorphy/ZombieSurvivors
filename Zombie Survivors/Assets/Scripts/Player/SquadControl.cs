using DG.Tweening;
using System;
using UnityEngine;

public class SquadControl : MonoBehaviour
{
	[SerializeField] private GameObject comradePrefab;
	[SerializeField] private Transform squad;
	private Player player;

	[Range(0f, 1f)]
	[SerializeField] private float DistanceFactor, Radius;

	[SerializeField] private int squadAmmount;

	public event Action<SquadControl, int> OnSquadIncrease;

	private void Awake()
	{
		player = GetComponent<Player>();
	}

	private void Start()
	{
		squadAmmount = 1;
	}

	public void IncreaseSquadSize(int size, bool multiply)
	{
		if (multiply == false)
			squadAmmount += size;
		else
			squadAmmount *= size;

		CreateComrades(squadAmmount);
	}

	private void ApplyMultiplication(int comradesAmmount)
	{
		for (int i = 0; i < comradesAmmount; i++)
		{
			var x = DistanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * Radius);
			var z = DistanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * Radius);

			var newPos = new Vector3(x, transform.position.y, z);

			squad.transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
		}
	}

	public void CreateComrades(int comradesAmmount)
	{
		for (int i = 0; i < comradesAmmount; i++)
		{
			GameObject comObj = Instantiate(comradePrefab, transform.position, Quaternion.identity, squad);
			comObj.GetComponent<Comrade>().SetPlayerReference(player);
		}

		ApplyMultiplication(comradesAmmount);

		OnSquadIncrease?.Invoke(this, squadAmmount);
	}

	public int GetSquadAmmount()
	{
		return squadAmmount;
	}
}
