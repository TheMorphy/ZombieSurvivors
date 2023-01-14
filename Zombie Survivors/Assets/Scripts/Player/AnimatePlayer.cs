using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
	private Comrade comrade;

	public List<Collider> ragdollParts = new List<Collider>();

	private void Awake()
	{
		comrade = GetComponent<Comrade>();

		SetRagdollColliders();
	}

	private void Update()
	{
		comrade.Animator.SetFloat("MoveSpeed", comrade.Player.playerController.GetMoveSpeed());
	}

	public void TurnOnRagdoll()
	{
		transform.parent = null;
		SquadControl.ComradesTransforms.Remove(transform);

		comrade.FireWeapon.enabled = false;
		comrade.Animator.enabled = false;
		comrade.Animator.avatar = null;

		gameObject.GetComponent<CapsuleCollider>().enabled = false;

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
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
}
