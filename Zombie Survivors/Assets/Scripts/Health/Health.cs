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

	/// <summary>
	/// Increase health by specified percent
	/// </summary>
	public void AddHealth(int healthPercent)
	{
		int healthIncrease = Mathf.RoundToInt((startingHealth * healthPercent) / 100f);

		int totalHealth = currentHealth + healthIncrease;

		if (totalHealth > startingHealth)
		{
			currentHealth = startingHealth;
		}
		else
		{
			currentHealth = totalHealth;
		}

		CallHealthEvent(0);
	}

}
