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
	public List<CardSO> LegendaryCards;

	[Space(5)]
	[Header("END GAME")]
	public GameObject EvacuationArea;

	public Sprite GetAirdropSprite(AirdropType airdropType)
	{
		return Airdrops.Find(x => x.AirdropType.Equals(airdropType)).AirdropSprite;
	}

	public Sprite GetCardSprite(CardType cardType, CardRarity cardRarity)
	{
		Sprite sprite = null;
		string code = cardType.ToString() + "_" + cardRarity.ToString();

		switch (cardRarity)
		{
			case CardRarity.Common:
				sprite = CommonCards.Find(x => x.CardCode.Equals(code)).CardSprite;
				break;
			case CardRarity.Rare:
				sprite = RareCards.Find(x => x.CardCode.Equals(code)).CardSprite;
				break;
			case CardRarity.Epic:
				sprite = EpicCards.Find(x => x.CardCode.Equals(code)).CardSprite;
				break;
			case CardRarity.Legendary:
				sprite = Instance.LegendaryCards.Find(x => x.CardCode.Equals(code)).CardSprite;
				break;
		}
		return sprite;
	}
}
