using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
	private static GameResources instance;

	public static GameResources Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load<GameResources>("GameResources");
			}
			return instance;
		}
	}

	//#region PLAYER SELECTION
	//[Space(10)]
	//[Header("PLAYER SELECTION")]
	//#endregion PLAYER SELECTION
	//#region Tooltip
	//[Tooltip("The PlayerSelection prefab")]
	//#endregion Tooltip
	//public GameObject playerSelectionPrefab;

	#region Header PLAYER
	[Space(10)]
	[Header("PLAYER")]
	#endregion Header PLAYER
	#region Tooltip
	[Tooltip("Player details list - populate the list with the playerdetails scriptable objects")]
	#endregion Tooltip
	public List<PlayerDetailsSO> playerDetailsList;
	#region Tooltip
	[Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
	#endregion Tooltip
	public CurrentPlayerSO currentPlayer;
}
