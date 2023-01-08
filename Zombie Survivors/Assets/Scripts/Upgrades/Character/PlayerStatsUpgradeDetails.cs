using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsUpgrade_", menuName = "Scriptable Objects/Upgrades/Player")]
public class PlayerStatsUpgradeDetails : ScriptableObject
{
	[Space(5)]
	[Header("SELECT WHICH STAT TO UPGRADE")]

	[HideInInspector] public UpgradeType UpgradeType = UpgradeType.PlayerStatUpgrade;

	public PlayerStats PlayerStats;

	public float FloatValue;
	public bool Toggle;
	public int IntValue;

	public UpgradeAction UpgradeAction;

	#region EDITOR

#if UNITY_EDITOR

	[CustomEditor(typeof(PlayerStatsUpgradeDetails))]
	public class UpgradeEditor : Editor
	{
		private PlayerStatsUpgradeDetails playerStatUpgradeDetails;

		private void OnEnable()
		{
			playerStatUpgradeDetails = target as PlayerStatsUpgradeDetails;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Space(10);

			playerStatUpgradeDetails.PlayerStats = (PlayerStats)EditorGUILayout.EnumPopup("Player Stats", playerStatUpgradeDetails.PlayerStats);

			switch (playerStatUpgradeDetails.PlayerStats)
			{
				// FLOAT Value Fields
				case PlayerStats.Health:
				case PlayerStats.MoveSpeed:

					playerStatUpgradeDetails.FloatValue = EditorGUILayout.FloatField("Value", playerStatUpgradeDetails.FloatValue);

					playerStatUpgradeDetails.UpgradeAction = (UpgradeAction)EditorGUILayout.EnumPopup("Value Action", playerStatUpgradeDetails.UpgradeAction);

					EditorGUILayout.Space(5);

					break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

#endif

	#endregion
}
