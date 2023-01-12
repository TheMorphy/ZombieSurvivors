using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ComradeMovement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Comrade : MonoBehaviour
{
    private Player player;
	private Animator animator;
	private Health comradeHealth;
	private HealthEvent comradeHealthEven;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		comradeHealthEven = GetComponent<HealthEvent>();
		comradeHealth = GetComponent<Health>();
	}

	private void OnEnable()
	{
		comradeHealthEven.OnHealthChanged += HealthEvent_OnHealthChanged;
	}

	private void OnDisable()
	{
		comradeHealthEven.OnHealthChanged -= HealthEvent_OnHealthChanged;
	}

	/// <summary>
	/// Handle health changed event
	/// </summary>
	private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		// If player has died
		if (healthEventArgs.healthAmount <= 0f)
		{
			animator.SetTrigger("Die");
		}
	}

	public void SetComradeHealth(int ammount)
	{
		comradeHealth.SetStartingHealth(ammount);
	}

	public void SetAnimator(Animator playerAnimator)
	{
		animator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;

		AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
		string currentClipName = clipInfo[0].clip.name;

		animator.Play(currentClipName, 0, playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
	}

	public void SetPlayerReference(Player player)
	{
		this.player = player;

		SetAnimator(player.animator);

		SetComradeHealth(player.health.GetStartingHealth());
	}

	public Player GetPlayer()
	{
		return player;
	}
}
