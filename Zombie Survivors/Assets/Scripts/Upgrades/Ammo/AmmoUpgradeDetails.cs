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
			serializedObject.Update();

			EditorGUILayout.Space(10);

			ammoUpgradeDetails.AmmoStats = (AmmoStats)EditorGUILayout.EnumPopup("Ammo Stats", ammoUpgradeDetails.AmmoStats);

			switch (ammoUpgradeDetails.AmmoStats)
			{
				// FLOAT Value Fields
				case AmmoStats.AmmoDamage:
				case AmmoStats.AmmoPerShot:
				case AmmoStats.AmmoSpeed:
				case AmmoStats.AmmoRange:
				case AmmoStats.AmmoShootAngle:

					ammoUpgradeDetails.FloatValue = EditorGUILayout.FloatField("Value", ammoUpgradeDetails.FloatValue);

					ammoUpgradeDetails.UpgradeAction = (UpgradeAction)EditorGUILayout.EnumPopup("Value Action", ammoUpgradeDetails.UpgradeAction);

					EditorGUILayout.Space(5);

					break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}

#endif

	#endregion
}
