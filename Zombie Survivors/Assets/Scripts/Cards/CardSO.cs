using UnityEngine;

public enum CardType
{
	Weapon,
	Gloves,
	Helmet,
	Armor,
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

	[SerializeField] private string cardName;
	public string CardName { get { return cardName; } }

	[SerializeField] private Sprite cardSprite;
	public Sprite CardSprite { get { return cardSprite; } }

	[SerializeField] private AnimationCurve scallingConfiguration;
	public AnimationCurve ScallingConfiguration { get { return scallingConfiguration; } }

	[Space]
	[SerializeField] WeaponStats upgradeStat;
	public WeaponStats UpgradeStat { get { return upgradeStat; } }

	[SerializeField] UpgradeAction upgradeAction;
	public UpgradeAction UpgradeAction { get { return upgradeAction; } }

	[SerializeField] private float upgradeAmmount;
	public float UpgradeAmmount { get { return upgradeAmmount; } }

	[HideInInspector] public string CardCode => CardName + "_" + CardRarity.ToString();
}
