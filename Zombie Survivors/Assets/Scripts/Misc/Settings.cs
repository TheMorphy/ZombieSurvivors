using UnityEngine;

public static class Settings
{
	#region ANIMATOR PARAMETERS
	// Animator parameters - Player
	public static int MoveSpeed = Animator.StringToHash("MoveSpeed");
	public static int AttackIndex = Animator.StringToHash("AttackIndex");

	public static int AttacksCount = 4;
	#endregion
}
