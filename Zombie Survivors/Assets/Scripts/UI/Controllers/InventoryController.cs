using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Button sortByRarityBtn;
    [SerializeField] private GameObject cardReference;

	[SerializeField] private int inventorySize = 10;

	private void Start()
	{
		sortByRarityBtn.onClick.AddListener(() => {
			//TODO: Sort
		});
	}

    public void AddToInventory(CardDTO cardDetails)
    {
        for (int i = 0; i < transform.childCount; i++)
		{
			Card cardSlot = transform.GetChild(i).GetComponent<Card>();
			if (cardSlot.IsEmpty)
            {
                cardSlot.InitializeCard(cardDetails);
			}
        }
    }

    public void InitializeEmptyInventory()
    {
		if (inventorySize == transform.childCount)
			return;

        for (int i = 0; i < inventorySize; i++)
        {
			GameObject cardObj = Instantiate(cardReference, transform);
			cardObj.SetActive(true);
		}
    }
}
