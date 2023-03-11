using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquadControl : MonoBehaviour
{
	[SerializeField] private GameObject comradePrefab;

	[Range(0f, 1f)]
	[SerializeField] private float DistanceFactor, Radius;

	[SerializeField] private int squadAmmount = 0;

	public event Action<SquadControl, SquadControlEventArgs> OnSquadAmmountChanged;

	public static List<Transform> ComradesTransforms = null;

	private BoxCollider collider;
	private float timer;

	private void Awake()
	{
		ComradesTransforms = new List<Transform>();
		collider = GetComponent<BoxCollider>();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if(timer > 1f)
		{
			UpdateColliderSize();
			timer = 0;
		}
	}

	public void CreateFirstComrade()
	{
		Comrade comrade = Instantiate(comradePrefab, transform.position, Quaternion.identity, transform).GetComponent<Comrade>();
		comrade.InitializeComrade();

		squadAmmount = ComradesTransforms.Count;
		CallSquadChangedEvent(squadAmmount);

		comrade.WeaponFiredEvent.OnWeaponFired += WeaponFiredEvent_OnWeaponFired;
	}

	/// <summary>
	/// Play audio only for one comrade weapon, because if we try to play same shooting sound for every comrade,
	/// it will cause massive volume spike and sound glitching
	/// </summary>
	private void WeaponFiredEvent_OnWeaponFired(WeaponFiredEvent arg1, WeaponFiredEventArgs arg2)
	{
		AudioManager.Instance.PlaySFX(SoundTitle.Gun_Shoot);
	}

	public static Bounds GetChildrenBounds(Transform transform)
	{
		Bounds bounds = new Bounds(transform.position, Vector3.zero);

		for (int i = 1; i < transform.childCount; i++)
		{
			bounds.Encapsulate(transform.GetChild(i).GetComponent<Collider>().bounds);
		}
		return bounds;
	}

	/// <summary>
	/// Updates the Player collider, depending on the ammount of comrades.
	/// I'm using this, instead of individual comrade colllider, to avoid calling events on collision multiple times, for each comrade
	/// </summary>
	private void UpdateColliderSize()
	{
		Bounds childrenBounds = GetChildrenBounds(transform);
		Vector3 boundingBoxMin = childrenBounds.min;
		Vector3 boundingBoxMax = childrenBounds.max;
		float boundingWidthX = (boundingBoxMax.x - boundingBoxMin.x);
		float boundingWidthZ = (boundingBoxMax.z - boundingBoxMin.z);

		collider.size = new Vector3(boundingWidthX, collider.size.y, boundingWidthZ);
	}

	public Vector3 FurthestMemberPosition()
	{
		if (ComradesTransforms.Count > 0)
			return ComradesTransforms.OrderByDescending(t => Vector3.Distance(transform.position, t.position)).FirstOrDefault().position;
		return transform.position;
	}

	/// <summary>
	/// Called in MultiplicationAreaController, to increase squad size
	/// </summary>
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

	/// <summary>
	/// Forms child objects in spiral formation
	/// </summary>
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

	/// <summary>
	/// Spawns new soldiers by specified ammount
	/// </summary>
	public void CreateComrades(int number)
	{
		for (int i = squadAmmount; i < number; i++)
		{
			Comrade comrade = Instantiate(comradePrefab, transform.position, Quaternion.identity, transform).GetComponent<Comrade>();
			comrade.InitializeComrade();
			comrade.name = $"Comrade_{i}";
		}

		squadAmmount = ComradesTransforms.Count;

		FormatSquad();

		CallSquadChangedEvent(squadAmmount);
	}

	public IEnumerator MoveTransformsToPosition(Vector3 position)
	{
		DisableComrades();

		position = Vector3.zero;

		var arangedComrades = ComradesTransforms.OrderBy(x => Vector3.Distance(x.transform.position, position)).ToList();

		foreach (var comrade in arangedComrades)
		{
			var moveTween = comrade.DOMove(position, 0.7f);
			Vector3 startScale = comrade.localScale;
			bool eventCalled = false;
			moveTween.OnUpdate(() =>
			{
				comrade.localScale = Vector3.Lerp(startScale, startScale / 2, moveTween.Elapsed());

				if (Vector3.Distance(comrade.transform.position, position) < 0.2f && !eventCalled)
				{
					StaticEvents.CallComradeBoardedEvent();
					eventCalled = true;
				}
			});

			yield return moveTween.WaitForCompletion();
		}

		GameManager.Instance.ChangeGameState(GameState.GameWon);
	}

	public void DisableComrades()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponents<MonoBehaviour>().ToList().ForEach(x => x.enabled = false);
		}
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
