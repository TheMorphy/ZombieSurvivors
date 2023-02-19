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

	#region Header PLAYER
	[Space(5)]
	[Header("PLAYER")]
	#endregion Header PLAYER
	#region Tooltip
	[Tooltip("Player details list - populate the list with the playerdetails scriptable objects")]
	#endregion Tooltip
	public List<PlayerDetailsSO> PlayerDetailsList;
	#region Tooltip
	[Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
	#endregion Tooltip
	public CurrentPlayerSO CurrentPlayer;

	#region Header GAMEPLAY
	[Space(5)]
	[Header("GAMEPLAY")]
	#endregion Header EXPERIENCE
	public GameObject ExpDrop;
	public GameObject MultiplicationCircle;
	[Space(1)]
	[Header("AIRDROPS")]
	public List<AirdropDetails> Airdrops;

	[Space(5)]
	[Header("UPGRADE CARDS")]
	public List<CardSO> CommonCards;
	public List<CardSO> RareCards;
	public List<CardSO> EpicCards;

	[Space(5)]
	[Header("END GAME")]
	public GameObject EvacuationArea;
}
