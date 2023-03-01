using UnityEngine;

public class HeaderTab : Tab
{
	[SerializeField] private CurrencyController currencyController;

	public override void Initialize(object[] args = null)
	{
		currencyController.InitializeCurrency();
	}
}
