using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI cardAmmountText;



    public void InitializeRewardCard(Sprite cardSprite, int cardAmmount)
    {
        cardImage.sprite = cardSprite;
		cardAmmountText.text = cardAmmount.ToString();

	}
}
