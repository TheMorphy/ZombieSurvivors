using System;
using System.Collections.Generic;

[Serializable]
public class ActiveUpgrades
{
    public int ExtraDamage;
    public int ExtraHealth;
    public float ExtraMoveSpeed;
    public float ExtraAmmoSpread;
    public float ExtraFireRate;

    public void SetUpgrades(List<CardDTO> upgradeCardsDetails) 
    {
        foreach (var upgradeCardDetails in upgradeCardsDetails)
        {
            switch (upgradeCardDetails.CardType)
            {
                case CardType.Armor:

                    break;
				case CardType.Boots:

					break;
				case CardType.Helmet:

					break;
				case CardType.Gloves:

					break;
				case CardType.Weapon:

					break;
			}
        }
    }

    private void AddUpgrade(CardDTO upgradeCardDetails)
    {

    }
}
