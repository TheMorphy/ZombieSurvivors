using System;
using UnityEngine;

public enum CardType
{
	Shotgun,
	Rifle,
	Pistol,
	Gloves,
	Helmet,
	Armor,
	Boots,
}

public enum CardRarity
{
	Common,
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

	[Tooltip("Used for setting a gradually increasing value with each level. Time = level, Value = upgrade value")]
	[SerializeField]
	private SerializableAnimationCurve scalingConfigurationCurve = new SerializableAnimationCurve();

	public AnimationCurve ScallingConfiguration
	{
		get { return scalingConfigurationCurve.curve; }
		set { scalingConfigurationCurve.curve = value; }
	}

	[Space]
	[SerializeField] PlayerStats playerStats;
	public PlayerStats PlayerStat { get { return playerStats; } }

	[SerializeField] WeaponStats weaponStats;
	public WeaponStats WeaponStat { get { return weaponStats; } }

	[SerializeField] AmmoStats ammoStats;
	public AmmoStats AmmoStat { get { return ammoStats; } }

	[Space]
	[SerializeField] UpgradeAction upgradeAction;
	public UpgradeAction UpgradeAction { get { return upgradeAction; } }

	[HideInInspector] public string CardCode => CardType.ToString() + "_" + CardRarity.ToString();
}

[Serializable]
public class SerializableAnimationCurve
{
	public AnimationCurve curve;
}

#region For Serializing To File
[Serializable]
public class CardDTO
{
	public CardType CardType;
	public CardRarity CardRarity;
	public string CardName;
	public int CurrentCardLevel;
	public int CardsRequiredToNextLevel;
	public int Ammount;
	public AnimationCurve ScallingConfiguration;
	public WeaponStats WeaponStat;
	public PlayerStats PlayerStat;
	public AmmoStats AmmoStat;
	public UpgradeAction UpgradeAction;
	public CardSlot CardSlot;
	public string Code;

	public override bool Equals(object obj)
	{
		if (obj == null || !this.GetType().Equals(obj.GetType()))
		{
			return false;
		}

		CardDTO other = (CardDTO)obj;
		return this.Code == other.Code;
	}

	public override int GetHashCode()
	{
		return this.Code.GetHashCode();
	}

	public void LevelUp()
	{
		if (Ammount >= CardsRequiredToNextLevel)
		{
			Ammount -= CardsRequiredToNextLevel;
			CardsRequiredToNextLevel *= 2;
			CurrentCardLevel++;
			SaveManager.SaveToJSON(this, Settings.CARDS);
			AudioManager.Instance.PlaySFX(SoundTitle.Card_Upgrade);
		}
	}

	public bool CanLevelUpCard()
	{
		int cardsRequiredForNextLevel = CardsRequiredToNextLevel;
		int cardsNeededForUpgrade = cardsRequiredForNextLevel - Ammount;
		return cardsNeededForUpgrade <= Ammount;
	}


	public void Upgrade<T>(ref T stat, UpgradeAction action)
	{
		if (typeof(T) == typeof(int))
		{
			int intValue = (int)(object)stat;
			switch (action)
			{
				case UpgradeAction.Add:
					intValue = Mathf.FloorToInt(intValue + ScallingConfiguration.Evaluate(CurrentCardLevel));
					break;
				case UpgradeAction.Multiply:
					intValue = Mathf.FloorToInt(intValue * ScallingConfiguration.Evaluate(CurrentCardLevel));
					break;
			}
			stat = (T)(object)intValue;
		}
		else if (typeof(T) == typeof(float))
		{
			float floatValue = (float)(object)stat;
			switch (action)
			{
				case UpgradeAction.Add:
					floatValue += ScallingConfiguration.Evaluate(CurrentCardLevel);
					break;
				case UpgradeAction.Multiply:
					floatValue *= ScallingConfiguration.Evaluate(CurrentCardLevel);
					break;
			}
			stat = (T)(object)floatValue;
		}
		else
		{
			throw new NotSupportedException("Type " + typeof(T).Name + " is not supported.");
		}
	}
}
#endregion