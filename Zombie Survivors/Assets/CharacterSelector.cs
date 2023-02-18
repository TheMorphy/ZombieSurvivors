using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	[SerializeField] private Image characterPortrait;
	[SerializeField] private RectTransform characterPanel;

	public Sprite[] characterSprites;

	private int currentCharacterIndex = 0;

	private List<PlayerDetailsSO> playerDetails;
	private CurrentPlayerSO currentPlayer;

	public void InitializeChatacter()
	{
		playerDetails = GameResources.Instance.PlayerDetailsList;
		currentPlayer = GameResources.Instance.CurrentPlayer;

		characterSprites = new Sprite[playerDetails.Count];

		for (int i = 0; i < playerDetails.Count; i++)
		{
			characterSprites[i] = playerDetails[i].PlayerPicture;
		}

		characterPortrait.sprite = currentPlayer.playerDetails.PlayerPicture;
	}

	public void SelectNextCharacter()
	{
		currentCharacterIndex++;
		if (currentCharacterIndex >= characterSprites.Length)
		{
			currentCharacterIndex = 0;
		}

		currentPlayer.playerDetails = playerDetails[currentCharacterIndex];
		characterPortrait.sprite = characterSprites[currentCharacterIndex];
	}

	public void SelectPreviousCharacter()
	{
		currentCharacterIndex--;
		if (currentCharacterIndex < 0)
		{
			currentCharacterIndex = characterSprites.Length - 1;
		}

		currentPlayer.playerDetails = playerDetails[currentCharacterIndex];
		characterPortrait.sprite = characterSprites[currentCharacterIndex];
	}



	private void OnDisable()
	{
		GameResources.Instance.CurrentPlayer = currentPlayer;

		PlayerPrefs.SetString(Settings.CHARACTER_NAME, currentPlayer.playerName);

	}
}
