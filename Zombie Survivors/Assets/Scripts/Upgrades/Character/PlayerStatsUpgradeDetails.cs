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
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space(10);

			playerStatUpgradeDetails.PlayerStats = (PlayerStats)EditorGUILayout.EnumPopup("Player Stats", playerStatUpgradeDetails.PlayerStats);

			switch (playerStatUpgradeDetails.PlayerStats)
			{
				// FLOAT Value Fields
				case PlayerStats.Health:
				case PlayerStats.MoveSpeed:

					playerStatUpgradeDetails.UpgradeAction = (UpgradeAction)EditorGUILayout.EnumPopup("Value Action", playerStatUpgradeDetails.UpgradeAction);

					if(playerStatUpgradeDetails.UpgradeAction != UpgradeAction.Toggle)
					{
						playerStatUpgradeDetails.FloatValue = EditorGUILayout.FloatField("Value", playerStatUpgradeDetails.FloatValue);
					}
					else
					{
						EditorGUILayout.HelpBox("Invalid value selected", MessageType.Error);
					}

					EditorGUILayout.Space(5);

					break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(playerStatUpgradeDetails);
			}
		}
	}

#endif

	#endregion
}
