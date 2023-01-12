using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpandSystem : MonoBehaviour
{
    private TextMeshPro multiplyCountUI;
    private int randomNumber;
    private bool IsMultiplied;

	private void Awake()
	{
		multiplyCountUI = GetComponentInChildren<TextMeshPro>();
	}

	private void Start()
	{
		GenerateNewValue();
	}

	private void GenerateNewValue()
	{
		if (Random.value > 0.5f)
			IsMultiplied = false;
		else
			IsMultiplied = true;


		if (IsMultiplied)
		{
			randomNumber = Random.Range(2, 6);
			multiplyCountUI.text = "X" + randomNumber.ToString();
		}
		else
		{
			randomNumber = Random.Range(5, 20);

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
			SquadControl squadControl = other.GetComponentInParent<SquadControl>();

			squadControl.IncreaseSquadSize(randomNumber, IsMultiplied);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GenerateNewValue();
		}
	}
}
