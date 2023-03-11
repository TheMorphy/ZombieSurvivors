using UnityEngine;

#region Required Components
[RequireComponent(typeof(ComradeMovement))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ReceiveContactDamage))]
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
#endregion

public class Comrade : MonoBehaviour
{
	[HideInInspector] public Player Player;
	[HideInInspector] public Animator Animator;
	[HideInInspector] public AnimatePlayer AnimatePlayer;
	[HideInInspector] public ComradeMovement ComradeMovement;
	[HideInInspector] public Health Health;
	[HideInInspector] public FireWeapon FireWeapon;
	[HideInInspector] public ActiveWeapon ActiveWeapon;
	[HideInInspector] public WeaponFiredEvent WeaponFiredEvent;
	[HideInInspector] public ReloadWeaponEvent ReloadWeaponEvent;
	[HideInInspector] public SetActiveWeaponEvent SetActiveWeaponEvent;
	[HideInInspector] public HealthEvent HealthEvent;

	private void Awake()
	{
		Player = GetComponentInParent<Player>();
		Animator = GetComponent<Animator>();
		ComradeMovement = GetComponent<ComradeMovement>();
		AnimatePlayer = GetComponent<AnimatePlayer>();
		HealthEvent = GetComponent<HealthEvent>();
		ActiveWeapon = GetComponent<ActiveWeapon>();
		WeaponFiredEvent = GetComponent<WeaponFiredEvent>();
		ReloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
		SetActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
		FireWeapon = GetComponent<FireWeapon>();
		Health = GetComponent<Health>();
	}

	public void InitializeComrade()
	{
		SquadControl.ComradesTransforms.Add(transform);

		SetActiveWeaponEvent.CallSetActiveWeaponEvent(Player.PlayerWeapon);

		Health.SetStartingHealth(Player.PlayerDetails.Health);
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
		Health.UpdateHealthBar(healthEventArgs.healthPercent);
		StartCoroutine(Health.ShowHealthBarForSeconds(1f));

		if (healthEventArgs.healthAmount <= 0f)
		{
			Player.SquadControl.RemoveFromSquad(transform);

			AnimatePlayer.TurnOnRagdoll();		
		}
		AudioManager.Instance.PlaySFX(SoundTitle.Soldier_Hurt);
	}

	public void KillComrade()
	{
		Destroy(gameObject);
	}
}
