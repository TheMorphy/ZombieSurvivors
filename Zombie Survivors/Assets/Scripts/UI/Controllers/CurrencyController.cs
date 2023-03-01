using TMPro;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldAmmount;
    [SerializeField] private TextMeshProUGUI gemAmmount;

    public int Gems;
    public int Gold;

    public void InitializeCurrency()
    {
        Gems = PlayerPrefs.GetInt(Settings.GEMS, 20);    // If no vlaues have been set, the starting gem ammont is 20
        Gold = PlayerPrefs.GetInt(Settings.MONEY, 100);  // If no vlaues have been set, the starting gold ammont is 100

		goldAmmount.text = Gold.ToString();
		gemAmmount.text = Gems.ToString();
    }

    public void UseGems(int useAmmount)
    {
        Gems -= useAmmount;
		gemAmmount.text = Gems.ToString();
	}

	public void UseGold(int useAmmount)
	{
		Gold -= useAmmount;
		goldAmmount.text = Gold.ToString();
	}
}
