using TMPro;
using UnityEngine;

public class PlayerSquadAmmountUI : MonoBehaviour
{
	[SerializeField] private GameObject HealthBar;
	[SerializeField] private TextMeshPro squadAmmountText;

	private SquadControl squadControl;
	private Player player;

	private void Awake()
	{
		squadControl = GetComponentInParent<SquadControl>();
		player = GetComponentInParent<Player>();
	}

	private void Start()
	{
		squadAmmountText.text = squadControl.GetSquadAmmount().ToString();
	}

	private void OnEnable()
	{
		player.healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;

		squadControl.OnSquadAmmountChanged += PlayerController_OnSquadIncrease;
	}

	private void OnDisable()
	{
		player.healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;

		squadControl.OnSquadAmmountChanged -= PlayerController_OnSquadIncrease;
	}

	private void PlayerController_OnSquadIncrease(SquadControl SquadControlEvent, SquadControlEventArgs squadControlEventArgs)
	{
		squadAmmountText.text = squadControlEventArgs.squadSize.ToString();
	}

	private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
	{
		if (healthEventArgs.healthPercent <= 0f)
		{
			healthEventArgs.healthPercent = 0f;
		}

		HealthBar.transform.localScale = new Vector2(HealthBar.transform.localScale.x, healthEventArgs.healthPercent);
	}
}
