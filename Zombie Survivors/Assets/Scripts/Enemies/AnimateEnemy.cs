using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
	private Enemy enemy;
	private Animator animator;

	private Rigidbody[] rigidBodies;
	public List<Collider> ragdollParts = new List<Collider>();

	private float counter;
	private float duration = 2f;
	
	private void Awake()
	{
		enemy = GetComponent<Enemy>();
		animator = GetComponent<Animator>();
		rigidBodies = GetComponentsInChildren<Rigidbody>();
	}

	private void Start()
	{
		SetRagdollColliders();
		DeactivateRagdoll();
	}

	private void Update()
	{
		animator.SetFloat(Settings.MoveSpeed, enemy.enemyController.GetMoveSpeed());
	}

	public void StartAttacking()
	{
		animator.SetInteger(Settings.AttackIndex, Random.Range(0, Settings.AttacksCount));
		animator.SetBool("Attacking", true);
	}

	public void StopAttacking()
	{
		animator.SetBool("Attacking", false);
	}


	private void DeactivateRagdoll()
	{
		enemy.animator.enabled = true;

		foreach (var rb in rigidBodies)
		{
			rb.isKinematic = true;
		}
	}
	
	private void ActivateRagdoll()
	{
		enemy.animator.enabled = false;
		enemy.animator.avatar = null;

		foreach (var rb in rigidBodies)
		{
			rb.isKinematic = false;
		}
	}

	public void TurnOnRagdoll()
	{
		ActivateRagdoll();

		for (int i = 0; i < ragdollParts.Count; i++)
		{
			ragdollParts[i].isTrigger = false;

			if (ragdollParts[i].attachedRigidbody != null)
			{
				ragdollParts[i].attachedRigidbody.velocity = Vector3.zero;
			}
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

			if(transform.localScale.x < 0.1f && !enemy.enemyController.ExpDropped)
			{
				enemy.enemyController.DropEXP();
			}

			yield return null;
		}

		Destroy(gameObject);
	}
}
