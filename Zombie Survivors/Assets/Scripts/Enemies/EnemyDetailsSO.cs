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

	/// <summary>
	/// Uses animation curve as multiplier instead of directly setting values
	/// </summary>
	public EnemyDetailsSO ScaleUpEnemies(ScalingConfigurationSO Scaling, int Level)
	{
		EnemyDetailsSO scaledUpenemy = CreateInstance<EnemyDetailsSO>();

		scaledUpenemy.Class = Class;
		scaledUpenemy.enemyPrefab = enemyPrefab;

		scaledUpenemy.Health = Mathf.FloorToInt(Health * Scaling.HealthCurve.Evaluate(Level));
		scaledUpenemy.Damage = Mathf.FloorToInt(Damage * Scaling.DamageCurve.Evaluate(Level));
		scaledUpenemy.EXP_Increase = Mathf.FloorToInt(EXP_Increase * Scaling.ExpCurve.Evaluate(Level));
		scaledUpenemy.MoveSpeed = MoveSpeed * Scaling.SpeedCurve.Evaluate(Level);
		
		return scaledUpenemy;
	}
}

public enum EnemyClass
{
	Zombie,
	Boss
}
