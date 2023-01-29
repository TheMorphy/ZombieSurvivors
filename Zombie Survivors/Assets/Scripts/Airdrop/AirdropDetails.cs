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
	public AirdropType airdropType;
	public GameObject airdropPackage;
}
