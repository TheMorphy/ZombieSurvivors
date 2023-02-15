using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;

    public void DisplayRewardCard(CardDTO reward)
    {
        cardImage.sprite = reward.CardSprite;
		cardAmmountText.text = reward.Ammount.ToString();
	}
}
