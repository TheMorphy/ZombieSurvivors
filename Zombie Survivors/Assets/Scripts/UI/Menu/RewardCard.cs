using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;

    public void InitializeRewardCard(Reward reward)
    {
        cardImage.sprite = reward.RewardDetails.CardSprite;
		cardAmmountText.text = reward.Ammount.ToString();
	}
}
