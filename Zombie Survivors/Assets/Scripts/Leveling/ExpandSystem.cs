using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpandSystem : MonoBehaviour
{
    private TextMeshPro multiplyCountUI;
    private int randomMultiply;
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
			randomMultiply = Random.Range(2, 5);
			multiplyCountUI.text = "X" + randomMultiply.ToString();
		}
		else
		{
			randomMultiply = Random.Range(5, 20);

			if (randomMultiply % 2 != 0)
			{
				randomMultiply += 1;
			}

			multiplyCountUI.text = "+" + randomMultiply.ToString();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			SquadControl squadControl = other.GetComponent<SquadControl>();

			squadControl.IncreaseSquadSize(randomMultiply, IsMultiplied);
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
