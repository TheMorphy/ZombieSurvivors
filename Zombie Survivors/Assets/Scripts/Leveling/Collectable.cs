using System.Collections;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
	private protected float collectTime = 0.2f;

	protected abstract void OnCollected();

	protected IEnumerator CollectRoutine(Transform playerTransform)
	{
		float elapsedTime = 0;

		while (elapsedTime < collectTime)
		{
			transform.position = Vector3.Lerp(transform.position, playerTransform.position, (elapsedTime / collectTime));
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (elapsedTime / collectTime));
			elapsedTime += Time.deltaTime;

			if (Vector3.Distance(transform.position, playerTransform.position) <= 0.1f)
			{
				OnCollected();
			}

			yield return null;
		}
	}

	public virtual void Collect(Transform target) 
	{ 
		StartCoroutine(CollectRoutine(target));
	}
}
