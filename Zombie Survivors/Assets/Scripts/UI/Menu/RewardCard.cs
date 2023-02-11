using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;

    public void InitializeReward(CardSO cardDetails, int cardAmmount)
    {
        cardImage.sprite = cardDetails.CardSprite;
		cardAmmountText.text = cardAmmount.ToString();

        // This returns card name mixed with card rarity
        string cardCode = cardDetails.CardCode;

		if (PlayerPrefs.HasKey(cardCode) == false)
        {
            PlayerPrefs.SetString(cardCode, cardCode);
            PlayerPrefs.SetInt(cardCode + "_" + Settings.CurrentCardLevel, 0);
            PlayerPrefs.SetInt(cardCode + "_" + Settings.CardsRequiredToLevelUp, 2); // 2 as default for cards needed to upgrade
            PlayerPrefs.SetInt(cardCode + "_" + Settings.CardAmmount, 0); // 0 because this would be the first card
        }
	}
}
