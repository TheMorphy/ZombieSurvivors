using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class ReceiveContactDamage : MonoBehaviour
{
	#region Header
	[Header("The contact damage amount to receive")]
	#endregion
	private int contactDamageAmount;
	private Health health;

	private void Awake()
	{
		//Load components
		health = GetComponent<Health>();
	}

	public void TakeContactDamage(int damageAmount = 0)
	{
		if (contactDamageAmount > 0)
			damageAmount = contactDamageAmount;

		health.TakeDamage(damageAmount);
	}
}