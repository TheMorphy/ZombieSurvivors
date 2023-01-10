
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[CreateAssetMenu(fileName = "WeaponUpgrade_", menuName = "Scriptable Objects/Upgrades/Weapon")]
public class WeaponUpgradeDetails : ScriptableObject
{
	[Space(5)]
	[Header("SELECT WHICH STAT TO UPGRADE")]

	[HideInInspector] public UpgradeType UpgradeType = UpgradeType.PlayerStatUpgrade;

	public WeaponStats WeaponStats;

	public float FloatValue;
	public int IntValue;
	public bool Toggle; 

	public UpgradeAction UpgradeAction;

	#region EDITOR

#if UNITY_EDITOR

	[CustomEditor(typeof(WeaponUpgradeDetails))]
	public class UpgradeEditor : Editor
	{
		private WeaponUpgradeDetails weaponUpgradeDetails;

		private void OnEnable()
		{
			weaponUpgradeDetails = target as WeaponUpgradeDetails;
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space(10);

			weaponUpgradeDetails.WeaponStats = (WeaponStats)EditorGUILayout.EnumPopup("Weapon Stats", weaponUpgradeDetails.WeaponStats);

			switch (weaponUpgradeDetails.WeaponStats)
			{
				// FLOAT Value Fields
				case WeaponStats.fireRate:
				case WeaponStats.burstInterval:
				case WeaponStats.weaponReloadTime:
				case WeaponStats.weaponAmmoCapacity:
				case WeaponStats.weaponClipAmmoCapacity:

					weaponUpgradeDetails.UpgradeAction = (UpgradeAction)EditorGUILayout.EnumPopup("Value Action", weaponUpgradeDetails.UpgradeAction);

					if (weaponUpgradeDetails.UpgradeAction != UpgradeAction.Toggle)
					{
						weaponUpgradeDetails.FloatValue = EditorGUILayout.FloatField("Value", weaponUpgradeDetails.FloatValue);
					}
					else
					{
						EditorGUILayout.HelpBox("Invalid value selected", MessageType.Error);
					}

					EditorGUILayout.Space(5);

					break;

				// BOOL Fields
				case WeaponStats.burstFire:
				case WeaponStats.spreadShot:
				case WeaponStats.hasInfiniteAmmo:
				case WeaponStats.hasInfiniteClipCapacity:

					weaponUpgradeDetails.Toggle = EditorGUILayout.Toggle("Enabled", weaponUpgradeDetails.Toggle);

					weaponUpgradeDetails.UpgradeAction = UpgradeAction.Toggle;

					EditorGUILayout.Space(5);

					break;
			}

			if (EditorGUI.EndChangeCheck())
			{
				EditorUtility.SetDirty(weaponUpgradeDetails);
			}
		}
	}

#endif

#endregion
}
