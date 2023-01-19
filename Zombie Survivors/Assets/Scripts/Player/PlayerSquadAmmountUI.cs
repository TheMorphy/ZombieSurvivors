using TMPro;
using UnityEngine;

public class PlayerSquadAmmountUI : MonoBehaviour
{
	[SerializeField] private GameObject HealthBar;
	[SerializeField] private TextMeshPro squadAmmountText;

	private SquadControl squadControl;

	private void Awake()
	{
		squadControl = GetComponentInParent<SquadControl>();
	}

	private void Start()
	{
		squadAmmountText.text = squadControl.GetSquadAmmount().ToString();
	}

	private void OnEnable()
	{
		squadControl.OnSquadAmmountChanged += PlayerController_OnSquadIncrease;
	}

	private void OnDisable()
	{
		squadControl.OnSquadAmmountChanged -= PlayerController_OnSquadIncrease;
	}

	private void PlayerController_OnSquadIncrease(SquadControl SquadControlEvent, SquadControlEventArgs squadControlEventArgs)
	{
		squadAmmountText.text = squadControlEventArgs.squadSize.ToString();
	}
}
