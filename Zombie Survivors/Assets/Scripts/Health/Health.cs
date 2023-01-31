using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
	public int StartingHealth;
	public int CurrentHealth;

	private HealthEvent healthEvent;

	bool isVisible = false;
	[SerializeField] private Transform healthBar;

	private Image bossHealth;

	private void Awake()
	{
		if(healthBar != null)
			healthBar.parent.gameObject.SetActive(false);

		healthEvent = GetComponent<HealthEvent>();
	}

	private void LateUpdate()
	{
		if (healthBar != null && !transform.tag.Contains("Boss"))
			healthBar.parent.transform.LookAt(Camera.main.transform);
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
		StartingHealth = startingHealth;
		CurrentHealth = startingHealth;
	}

	public void AddTotalHealth(int healthToAdd)
	{
		StartingHealth += healthToAdd;
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

	public IEnumerator ShowHealthBarForSeconds(float seconds)
	{
		if (!isVisible)
		{
			healthBar.parent.gameObject.SetActive(true);
			isVisible = true;
		}
		yield return new WaitForSeconds(seconds);
		healthBar.parent.gameObject.SetActive(false);
		isVisible = false;
	}

	public void UpdateHealthBar(float healthPercent)
	{
		if (healthPercent <= 0f)
		{
			healthPercent = 0f;
		}

		healthBar.localScale = new Vector2(healthPercent, healthBar.localScale.y);
	}

	/// <summary>
	/// Some stinky stuff in here. Remember to update
	/// </summary>
	public void UpdateBossHealthBar(float healthPercent)
	{
		if (healthPercent <= 0f)
		{
			healthPercent = 0f;
		}

		// This needs to be separated, but for now lets leave it here
		bossHealth.fillAmount = healthPercent;
	}

	public void SetHealthbar(Image bossHealthbar)
	{
		bossHealth = bossHealthbar;
	}
}
