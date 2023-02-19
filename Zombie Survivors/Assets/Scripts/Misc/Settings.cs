using UnityEngine;

public static class Settings
{
	#region ANIMATOR PARAMETERS
	// Animator parameters - Player
	public static int MoveSpeed = Animator.StringToHash("MoveSpeed");
	public static int AttackIndex = Animator.StringToHash("AttackIndex");

	public static int AttacksCount = 4;
	#endregion

	public const string CurrentCardLevel = "CurrentCardLevel";
	public const string CardsRequiredToLevelUp = "RequiredCardsAmmount";
	public const string CardAmmount = "CardsAmmount";

	public const string CARDS = "Cards.json";
	public const string AIRDROPS = "Ardrops.json";
	public const string TRACKABLES = "Trackables.json";

	public const string CHARACTER_NAME = "CharactarName";
}
