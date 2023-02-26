using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplicationCircleManager : MonoBehaviour
{
    private TextMeshPro multiplyCountUI;
    private int randomNumber;
    private bool IsMultiplied;

	[SerializeField] private Vector2Int minMaxMultiplied = new Vector2Int(2, 5);
	[SerializeField] private Vector2Int minMaxAddition = new Vector2Int(5, 20);

	//List<Transform> collidingObjects = new List<Transform>();

	private void Awake()
	{
		multiplyCountUI = GetComponentInChildren<TextMeshPro>();
	}

	private void Start()
	{
		GenerateNewValue();
	}

	private void Update()
	{
		if (GameManager.Instance.gameState == GameState.evacuating)
		{
			StaticEvents.CallCollectedEvent(transform.position);
			Destroy(gameObject);
		}
	}

	private void GenerateNewValue()
	{
		if (Random.value > 0.5f)
			IsMultiplied = false;
		else
			IsMultiplied = true;


		if (IsMultiplied)
		{
			randomNumber = Random.Range(minMaxMultiplied.x, minMaxMultiplied.y);
			multiplyCountUI.text = "X" + randomNumber.ToString();
		}
		else
		{
			randomNumber = Random.Range(minMaxAddition.x, minMaxAddition.y);

			if (randomNumber % 2 != 0)
			{
				randomNumber += 1;
			}

			multiplyCountUI.text = "+" + randomNumber.ToString();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			AudioManager.Instance.PlaySFX(SoundTitle.Squad_Multiplier);

			StaticEvents.CallCollectedEvent(transform.position);

			SquadControl squadControl = other.GetComponentInParent<SquadControl>();

			squadControl.IncreaseSquadSize(randomNumber, IsMultiplied);

			Destroy(gameObject);
		}
	}
}
