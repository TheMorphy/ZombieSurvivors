using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
	Weapon,
	Glove,
	Helmet,
	Armor,
	Vest,
	Boots
}

public enum CardRarity
{
	Commomn,
	Rare,
	Epic,
	Legendary
}

[CreateAssetMenu(fileName = "UpgradeCard_", menuName = "Scriptable Objects/Upgrade Cards/Card")]
public class CardSO : ScriptableObject
{
	[SerializeField] CardType cardType;
    public CardType CardType { get { return cardType; } }

	[SerializeField] CardRarity cardRarity;
	public CardRarity CardRarity { get { return cardRarity; } }

	[SerializeField] private Sprite cardSprite;
	public Sprite CardSprite { get { return cardSprite; } }

	[Space]
	[SerializeField] WeaponStats upgradeStat;
	public WeaponStats UpgradeStat { get { return upgradeStat; } }

	[SerializeField] UpgradeAction upgradeAction;
	public UpgradeAction UpgradeAction { get { return upgradeAction; } }

	[SerializeField] private float upgradeAmmount;
	public float UpgradeAmmount { get { return upgradeAmmount; } }
}
