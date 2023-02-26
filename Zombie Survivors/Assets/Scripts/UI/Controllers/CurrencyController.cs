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
        Gems = PlayerPrefs.GetInt(Settings.GEM_AMMOUNT, 20);
        Gold = PlayerPrefs.GetInt(Settings.GOLD_AMMOUNT, 100);

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
