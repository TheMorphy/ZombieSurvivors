using UnityEngine;

#region Required Components
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
		SquadControl.ComradesTransforms.Add(transform);

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
			Player.squadControl.RemoveFromSquad(transform);

			AnimatePlayer.TurnOnRagdoll();		
		}
	}

	public void KillComrade()
	{
		Destroy(gameObject);
	}
}
