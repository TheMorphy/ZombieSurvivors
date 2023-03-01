using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;

    public void DisplayRewardCard(CardDTO cardDTO)
    {
        cardImage.sprite = GameResources.Instance.GetCardSprite(cardDTO.CardType, cardDTO.CardRarity);
		cardAmmountText.text = cardDTO.Ammount.ToString();
	}
}
