using UnityEngine;
using UnityEngine.UI;

public class InventoryController : SlotsController<CardDTO>
{
    [SerializeField] private Button sortByRarityBtn;

	private void Start()
	{
		sortByRarityBtn.onClick.AddListener(() => {
			//TODO: Sort
		});
	}
}
