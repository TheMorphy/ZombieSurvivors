using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	[SerializeField] private Image characterPortrait;
	[SerializeField] private RectTransform characterPanel;

	private Sprite[] characterSprites;
	private int currentCharacterIndex = 0;

	private List<PlayerDetailsSO> availablePlayerDetails;
	private CurrentPlayerSO currentPlayer;

	public void InitializeChatacter()
	{
		GetAvailableCharacter();

		characterSprites = new Sprite[availablePlayerDetails.Count];

		for (int i = 0; i < availablePlayerDetails.Count; i++)
		{
			characterSprites[i] = availablePlayerDetails[i].PlayerPicture;
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

		SaveCharacter(availablePlayerDetails[currentCharacterIndex]);
		characterPortrait.sprite = characterSprites[currentCharacterIndex];
	}

	public void SelectPreviousCharacter()
	{
		currentCharacterIndex--;
		if (currentCharacterIndex < 0)
		{
			currentCharacterIndex = characterSprites.Length - 1;
		}

		SaveCharacter(availablePlayerDetails[currentCharacterIndex]);
		characterPortrait.sprite = characterSprites[currentCharacterIndex];
	}

	private void SaveCharacter(PlayerDetailsSO selectedDetails)
	{
		PlayerPrefs.SetString(Settings.SAVED_CHARACTER, selectedDetails.Name);
		GameResources.Instance.CurrentPlayer.playerDetails = selectedDetails;
	}

	private void GetAvailableCharacter()
	{
		availablePlayerDetails = GameResources.Instance.PlayerDetailsList;
		currentPlayer = GameResources.Instance.CurrentPlayer;

		if (PlayerPrefs.HasKey(Settings.SAVED_CHARACTER) == false)
		{
			PlayerPrefs.SetString(Settings.SAVED_CHARACTER, availablePlayerDetails[0].Name);
		}
		else
		{
			currentPlayer.playerDetails = 
				availablePlayerDetails.First(x => x.Name.Equals(PlayerPrefs.GetString(Settings.SAVED_CHARACTER)));
		}
	}
}
