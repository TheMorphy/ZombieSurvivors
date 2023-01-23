using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
	private Enemy enemy;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();
	}

	public void EnemyDeath()
	{
		GameObject exp = Instantiate(GameResources.Instance.ExpDrop, transform.position, Quaternion.identity);
		exp.GetComponent<ExpDrop>().SetExpValue(enemy.enemyDetails.EXP_Increase);

		enemy.destroyedEvent.CallDestroyedEvent(false, enemy.enemyDetails.EXP_Increase);
	}
}
