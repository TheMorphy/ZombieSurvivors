using UnityEngine;

public enum CardSlot
{
    Active,
    Inventory
}

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
	[HideInInspector] public CardView CardView;

    [HideInInspector] public int CurrentCardLevel = 0;
    [HideInInspector] public int CardsRequiredToNextLevel = 2;
    [HideInInspector] public int CardsInside = 0;

	public bool IsEmpty = true;
	public bool IsReadyToUpgrade = false;

	[HideInInspector] public CardSO CardDetails;

	private void Awake()
	{
		CardView = GetComponent<CardView>();
	}

	public void InitializeCard(CardSO cardDetails)
	{
		CardDetails = cardDetails;

		UpdateGearStats();

		CardView.UpdateCardView(cardDetails);
	}

	private void UpdateGearStats()
	{
		
	}
}
