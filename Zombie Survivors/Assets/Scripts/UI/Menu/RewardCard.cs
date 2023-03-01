using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;

    public void DisplayRewardCard(CardDTO cardDTO)
    {
        cardImage.sprite = Resources.Load<Sprite>(Settings.CARD_SPRITES_PATH + cardDTO.CardType + "_" + cardDTO.CardRarity);
		cardAmmountText.text = cardDTO.Ammount.ToString();
	}
}
