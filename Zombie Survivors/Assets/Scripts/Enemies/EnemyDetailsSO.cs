using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Characters/Enemy")]
public class EnemyDetailsSO : ScriptableObject
{
	public EnemyClass Class = EnemyClass.Zombie;

	public GameObject enemyPrefab;

	public int Health = 30;

	public float MoveSpeed = 3f;

	public int Damage = 10;

	public int EXP_Increase = 10;
}

public enum EnemyClass
{
	Zombie
}
