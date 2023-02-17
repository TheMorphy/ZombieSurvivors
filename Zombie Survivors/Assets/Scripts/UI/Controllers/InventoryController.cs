using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : SlotsController<CardDTO>
{
    [SerializeField] private Button sortByRarityBtn;
    [SerializeField] private GameObject cardReference;

	[SerializeField] private int inventorySize = 10;

	[HideInInspector] public static List<Card> Inventory = new List<Card>();

	private void Start()
	{
		sortByRarityBtn.onClick.AddListener(() => {
			//TODO: Sort
		});
	}
}
