using System;
using UnityEngine;

public enum AirdropType
{
	Gold,
	Iron,
	Wooden
}

[CreateAssetMenu(fileName = "AirdropDetails_", menuName = "Scriptable Objects/Airdrops/Airdrop Details")]
public class AirdropDetails : ScriptableObject
{
	[SerializeField] private AirdropType airdropType;
	public AirdropType AirdropType { get { return airdropType; } }

	[SerializeField] private Sprite airdropSprite;
	public Sprite AirdropSprite { get { return airdropSprite; } }

	[SerializeField] private GameObject airdropPrefab;
	public GameObject AirdropPackage { get { return airdropPrefab; } }

	[SerializeField] private int maxGemsAmmount;
	public int MaxGemsAmmount { get { return maxGemsAmmount; } }

	[SerializeField] private int minGemsAmmount;
	public int MinGemsAmmount { get { return minGemsAmmount; } }

	[SerializeField] private int minGoldAmmount;
	public int MinGoldAmmount { get { return minGoldAmmount; } }

	[SerializeField] private int maxGoldAmmount;
	public int MaxGoldAmmount { get { return maxGoldAmmount; } }

	[SerializeField] private int unlockDuration;
	public int UnlockDuration { get { return unlockDuration; } }

	[SerializeField] private int cardAmmount;
	public int CardAmmount { get { return cardAmmount; } }

	[SerializeField] private int unlockCost;
	public int UnlockCost { get { return unlockCost; } }

	[Tooltip("Time in seconds. Value of 3600 = 1h")]
	[SerializeField] private int removeTime = 1800;
	public int RemoveTime { get { return removeTime; } }

	public int GetGemCostByRemainingTime()
	{
		float unlockTimer = UnlockDuration;
		return (int)Mathf.Ceil((unlockTimer * 3600 ) / 2);
	}
}

#region For Serializing Airdrop Scriptable Object To File
[Serializable]
public class AirdropDTO
{
	public AirdropType AirdropType;
	public Sprite AirdropSprite;
	public GameObject AirdropPackage;
	public int MaxGemsAmmount;
	public int MinGemsAmmount;
	public int MinGoldAmmount;
	public int MaxGoldAmmount;
	public int UnlockDuration;
	public int CardAmmount;
	public int UnlockCost;
	public int RemoveTime;
	public string Code;

	public override bool Equals(object obj)
	{
		if (obj == null || !this.GetType().Equals(obj.GetType()))
		{
			return false;
		}

		AirdropDTO other = (AirdropDTO)obj;
		return this.Code == other.Code;
	}

	public override int GetHashCode()
	{
		return this.Code.GetHashCode();
	}

}
#endregion
