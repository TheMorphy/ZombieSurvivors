using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSquadAmmountUI : MonoBehaviour
{
	private TextMeshPro squadAmmountText;
	private SquadControl squadControl;

	private void Awake()
	{
		squadControl = GetComponentInParent<SquadControl>();
		squadAmmountText = GetComponentInChildren<TextMeshPro>();
	}

	private void Start()
	{
		squadAmmountText.text = squadControl.GetSquadAmmount().ToString();
	}

	private void OnEnable()
	{
		squadControl.OnSquadIncrease += PlayerController_OnSquadIncrease;
	}

	private void OnDisable()
	{
		squadControl.OnSquadIncrease -= PlayerController_OnSquadIncrease;
	}

	private void PlayerController_OnSquadIncrease(SquadControl SquadControlEvent, int squadSize)
	{
		squadAmmountText.text = squadSize.ToString();
	}

	//private void LateUpdate()
 //   {
	//	squadAmmountText.transform.LookAt(Camera.main.transform.position);

	//	squadAmmountText.transform.rotation = Quaternion.LookRotation(squadAmmountText.transform.position - Camera.main.transform.position);
	//}
}
