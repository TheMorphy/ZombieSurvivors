using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails", menuName = "Scriptable Objects/Characters/Player")]
public class PlayerDetailsSO : ScriptableObject
{
	public PlayerClass Class = PlayerClass.Soldier;

	public GameObject PlayerPrefab;

	public WeaponDetailsSO startingWeaponDetails;

	public int Health = 100;

	public float MoveSpeed = 5f;

	public int Damage = 5;
}

public enum PlayerClass
{
	Soldier,
	Juggernaut,
	Fast_Soldier
}
