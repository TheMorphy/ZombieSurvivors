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
