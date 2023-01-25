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
	public EnemyDetailsSO ScaleUpEnemiesByLevel(ScalingConfigurationSO Scaling, int Level)
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

	/// <summary>
	/// Bit different than scaling by Level. This one just sets values directly to the ones on the curve.
	/// </summary>
	public void ScaleUpEnemiesByTime(ScalingConfigurationSO Scaling, float Time)
	{
		Health = Mathf.FloorToInt(Scaling.HealthCurve.Evaluate(Time));
		Damage = Mathf.FloorToInt(Scaling.DamageCurve.Evaluate(Time));
		EXP_Increase = Mathf.FloorToInt(Scaling.ExpCurve.Evaluate(Time));
		MoveSpeed = Scaling.SpeedCurve.Evaluate(Time);
	}

}

public enum EnemyClass
{
	Zombie,
	Boss
}
