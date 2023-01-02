using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Characters/Enemy")]
public class EnemyDetailsSO : ScriptableObject
{
	public EnemyClass Class = EnemyClass.Zombie;

	public int Health = 30;

	public float MoveSpeed = 3f;

	public int Damage = 10;

	[Range(0f, 1f)]
	public float EXP_Increase = 0.2f;

	public int CashDrop = 50;
}

public enum EnemyClass
{
	Zombie
}
