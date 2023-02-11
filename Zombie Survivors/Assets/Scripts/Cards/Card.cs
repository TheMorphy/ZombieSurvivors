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
	private CardSO cardDetails;

	public CardSlot cardSlot;

    [HideInInspector] public int CurrentCardLevel = 0;
    [HideInInspector] public int CardsRequiredToNextLevel = 2;
    [HideInInspector] public int CardsInside = 0;

	public bool IsEmpty = true;
	public bool IsReadyToUpgrade = false;
	
	private void Awake()
	{
		CardView = GetComponent<CardView>();
	}

	public void InitializeCard(CardSO cardDetails)
	{
		IsEmpty = false;

		this.cardDetails = cardDetails;
		
		CardView.UpdateCardView(cardDetails);
	}

	private void UpgradeCards(CardSO cardDetails)
	{
		int savedCardLevel = PlayerPrefs.GetInt(cardDetails.CardCode + Settings.CurrentCardLevel);
		int cardsRequiredToLevelUp = PlayerPrefs.GetInt(cardDetails.CardCode + Settings.CardsRequiredToLevelUp);
		int cardsSaved = PlayerPrefs.GetInt(cardDetails.CardCode + Settings.CardAmmount);
	}
}
