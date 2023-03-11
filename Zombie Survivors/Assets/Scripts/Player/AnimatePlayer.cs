using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
	private Comrade comrade;

	public List<Collider> ragdollParts = new List<Collider>();

	private float counter;
	private float duration = 2f;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();

		SetRagdollColliders();
	}

	private void Update()
	{
		comrade.Animator.SetFloat(Settings.MoveSpeed, comrade.Player.PlayerController.GetMoveSpeed());
	}

	public void TurnOnRagdoll()
	{
		comrade.ComradeMovement.enabled = false;

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
		yield return new WaitForSeconds(2f);

		var joints = transform.GetComponentsInChildren<CharacterJoint>().ToList();
		joints.ForEach(x => x.breakTorque = 0.1f);
		joints.ForEach(x => x.breakForce = 0.1f);

		while (counter < duration)
		{
			counter += Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, counter / duration);
			yield return null;
		}

		// Displays Game Over screen if there are no comrades alive and the last one has finished the dying animation
		if (SquadControl.ComradesTransforms.Count == 0)
		{
			GameManager.Instance.ChangeGameState(GameState.GameLost);
		}

		Destroy(gameObject);
	}
}
