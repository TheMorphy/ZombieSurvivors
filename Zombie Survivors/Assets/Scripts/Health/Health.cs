using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
	[SerializeField] private int startingHealth;
	[SerializeField] private int currentHealth;
	private HealthEvent healthEvent;

	private void Awake()
	{
		healthEvent = GetComponent<HealthEvent>();
	}

	private void CallHealthEvent(int damageAmount)
	{
		// Trigger health event
		healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
	}

	/// <summary>
	/// Public method called when damage is taken
	/// </summary>
	public void TakeDamage(int damageAmount)
	{
		currentHealth -= damageAmount;
		CallHealthEvent(damageAmount);
	}

	/// <summary>
	/// Set starting health 
	/// </summary>
	public void SetStartingHealth(int startingHealth)
	{
		this.startingHealth = startingHealth;
		currentHealth = startingHealth;
	}

	/// <summary>
	/// Get the starting health
	/// </summary>
	public int GetStartingHealth()
	{
		return startingHealth;
	}

	public void UpgradPlayerHealth(float value, UpgradeAction upgradeAction)
	{
		if (upgradeAction == UpgradeAction.Add)
		{ 
			currentHealth += ((int)value);

		}
		else if (upgradeAction == UpgradeAction.Multiply)
		{
			currentHealth = ((int)(currentHealth * value));
		}
		else if (upgradeAction == UpgradeAction.Increase_Percentage)
		{
			currentHealth = ((int)Utilities.ApplyPercentage(value, currentHealth));
		}
			
	}

}
