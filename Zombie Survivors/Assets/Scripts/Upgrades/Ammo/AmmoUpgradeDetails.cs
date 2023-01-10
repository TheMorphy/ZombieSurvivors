using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade_", menuName = "Scriptable Objects/Upgrades/Ammo")]
public class AmmoUpgradeDetails : ScriptableObject
{
	[Space(5)]
	[Header("SELECT WHICH STAT TO UPGRADE")]

	[HideInInspector] public UpgradeType UpgradeType = UpgradeType.AmmoUpgrade;

	public AmmoStats AmmoStats;

	public float FloatValue;
	public bool Toggle;
	public int IntValue;

	public UpgradeAction UpgradeAction;

	#region EDITOR

#if UNITY_EDITOR

	[CustomEditor(typeof(AmmoUpgradeDetails))]
	public class UpgradeEditor : Editor
	{
		private AmmoUpgradeDetails ammoUpgradeDetails;

		private void OnEnable()
		{
			ammoUpgradeDetails = target as AmmoUpgradeDetails;
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space(10);

			ammoUpgradeDetails.AmmoStats = (AmmoStats)EditorGUILayout.EnumPopup("Ammo Stats", ammoUpgradeDetails.AmmoStats);

			switch (ammoUpgradeDetails.AmmoStats)
			{
				// FLOAT Value Fields
				case AmmoStats.AmmoDamage:
				case AmmoStats.AmmoPerShot:
				case AmmoStats.AmmoSpeed:
				case AmmoStats.AmmoRange:
				case AmmoStats.AmmoSpread:

					ammoUpgradeDetails.UpgradeAction = (UpgradeAction)EditorGUILayout.EnumPopup("Value Action", ammoUpgradeDetails.UpgradeAction);

					if (ammoUpgradeDetails.UpgradeAction != UpgradeAction.Toggle)
					{
						ammoUpgradeDetails.FloatValue = EditorGUILayout.FloatField("Value", ammoUpgradeDetails.FloatValue);
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
				EditorUtility.SetDirty(ammoUpgradeDetails);
			}
		}
	}

#endif

	#endregion
}
