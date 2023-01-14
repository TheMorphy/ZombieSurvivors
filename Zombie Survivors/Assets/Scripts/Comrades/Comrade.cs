using UnityEngine;

[RequireComponent(typeof(ComradeMovement))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Comrade : MonoBehaviour
{
	[HideInInspector] public Player Player;
	[HideInInspector] public Animator Animator;
	[HideInInspector] public AnimatePlayer AnimatePlayer;
	[HideInInspector] public Health Health;
	[HideInInspector] public FireWeapon FireWeapon;
	[HideInInspector] public ActiveWeapon ActiveWeapon;
	[HideInInspector] public SetActiveWeaponEvent SetActiveWeaponEvent;
	[HideInInspector] public HealthEvent HealthEvent;

	private void Awake()
	{
		Player = GetComponentInParent<Player>();
		Animator = GetComponent<Animator>();
		AnimatePlayer = GetComponent<AnimatePlayer>();
		HealthEvent = GetComponent<HealthEvent>();
		ActiveWeapon = GetComponent<ActiveWeapon>();
		SetActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		FireWeapon = GetComponent<FireWeapon>();
		Health = GetComponent<Health>();
	}

	private void Start()
	{
		SetActiveWeaponEvent.CallSetActiveWeaponEvent(Player.playerWeapon);

		Health.SetStartingHealth(Player.playerDetails.Health);
	}

	private void OnEnable()
	{
		HealthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
	}

	private void OnDisable()
	{
		HealthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
	}

	private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		if (healthEventArgs.healthAmount <= 0f)
		{
			AnimatePlayer.TurnOnRagdoll();
		}
	}
}
