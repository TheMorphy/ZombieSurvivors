using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
	public int StartingHealth;
	public int CurrentHealth;

	private HealthEvent healthEvent;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
	}

	private void CallHealthEvent(int damageAmount)
	{
		// Trigger health event
		healthEvent.CallHealthChangedEvent(((float)CurrentHealth / (float)StartingHealth), CurrentHealth, damageAmount);
	}

	/// <summary>
	/// Public method called when damage is taken
	/// </summary>
	public void TakeDamage(int damageAmount)
	{
		CurrentHealth -= damageAmount;
		CallHealthEvent(damageAmount);
	}

	/// <summary>
	/// Set starting health 
	/// </summary>
	public void SetStartingHealth(int startingHealth)
	{
		this.StartingHealth = startingHealth;
		CurrentHealth = startingHealth;
	}

	public void AddTotalHealth(int healthToAdd)
	{
		this.StartingHealth += healthToAdd;
		CurrentHealth += healthToAdd;

		CallHealthEvent(0);
	}

	/// <summary>
	/// Get the starting health
	/// </summary>
	public int GetStartingHealth()
	{
		return StartingHealth;
	}

	public void UpgradPlayerHealth(float value, UpgradeAction upgradeAction)
	{
		if (upgradeAction == UpgradeAction.Add)
		{ 
			CurrentHealth += ((int)value);

		}
		else if (upgradeAction == UpgradeAction.Multiply)
		{
			CurrentHealth = ((int)(CurrentHealth * value));
		}
		else if (upgradeAction == UpgradeAction.Increase_Percentage)
		{
			CurrentHealth = ((int)Utilities.ApplyPercentage(value, CurrentHealth));
		}
			
	}

}
