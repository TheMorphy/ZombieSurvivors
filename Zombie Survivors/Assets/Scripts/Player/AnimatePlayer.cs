using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
	private Comrade comrade;

	public List<Collider> ragdollParts = new List<Collider>();

	private float counter;
	private float duration = 4f;

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
		comrade.ComradeMovement.enabled = false;

		transform.parent = null;

		comrade.Animator.enabled = false;
		comrade.Animator.avatar = null;

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
		yield return new WaitForSeconds(3f);

		while (counter < duration)
		{
			counter += Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 2);
			yield return null;
		}

		Destroy(gameObject);
	}
}
