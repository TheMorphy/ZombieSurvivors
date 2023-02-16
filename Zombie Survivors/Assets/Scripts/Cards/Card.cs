using System;
using UnityEngine;

public enum CardSlot
{
    Active,
    Inventory
}

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
	public CardView CardView;

	public bool IsEmpty = true;
	public bool IsReadyToUpgrade = false;

	[HideInInspector] public CardDTO CardDetails;

	public void InitializeCard(CardDTO cardDetails)
	{
		IsEmpty = false;
		CardDetails = cardDetails;
		CardView.CardReference = this;
		CardView.UpdateCardView(cardDetails);
	}

	public void InitializeEmptyCard()
	{
		IsEmpty = true;
		CardDetails = null;
		CardView.InitializeEmptyView();
	}

	public void UpdateGearStats()
	{
		
	}

	public void RemoveFromActiveDeck()
	{
		
	}

	public void UseInActiveDeck()
	{
		
	}
}

#region For Serializing To File
[Serializable]
public class CardDTO
{
	public int ID;
	public CardType CardType;
	public CardRarity CardRarity;
	public string CardName;
	public Sprite CardSprite;
	public int CurrentCardLevel;
	public int CardsRequiredToNextLevel;
	public int Ammount;
	public float UpgradeValue;
	public WeaponStats UpgradeStat;
	public UpgradeAction UpgradeAction;
	public string CardCode;
}
#endregion