using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
	private Enemy enemy;

	public List<Collider> ragdollParts = new List<Collider>();

	private float counter;
	private float duration = 2f;
	private bool expDropped = false;

	private void Awake()
	{
		enemy = GetComponent<Enemy>();

		SetRagdollColliders();
	}

	private void EnemyDeath()
	{
		GameObject exp = Instantiate(GameResources.Instance.ExpDrop, transform.position, Quaternion.identity);
		exp.GetComponent<ExpDrop>().SetExpValue(enemy.enemyDetails.EXP_Increase);
		expDropped = true;
		enemy.destroyedEvent.CallDestroyedEvent(false, enemy.enemyDetails.EXP_Increase);
	}

	private void Update()
	{
		enemy.animator.SetFloat(Settings.MoveSpeed, enemy.enemyController.GetMoveSpeed());
	}

	public void TurnOnRagdoll()
	{
		enemy.animator.enabled = false;
		enemy.animator.avatar = null;

		gameObject.GetComponent<Collider>().enabled = false;

		for (int i = 0; i < ragdollParts.Count; i++)
		{
			ragdollParts[i].isTrigger = false;
			ragdollParts[i].attachedRigidbody.velocity = Vector3.zero;
		}
		
		StartCoroutine(Die());
	}

	private void SetRagdollColliders()
	{
		Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				colliders[i].isTrigger = true;
				ragdollParts.Add(colliders[i]);
			}
		}
	}

	private IEnumerator Die()
	{
		yield return new WaitForSeconds(2f);

		var joints = transform.GetComponentsInChildren<CharacterJoint>().ToList();
		joints.ForEach(x => x.breakTorque = 0.5f);
		joints.ForEach(x => x.breakForce = 0.5f);

		while (counter < duration)
		{
			counter += Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, counter / duration);

			if(transform.localScale.x < 0.1f && !expDropped)
			{
				EnemyDeath();
			}

			yield return null;
		}
		
		Destroy(gameObject);
	}
}
