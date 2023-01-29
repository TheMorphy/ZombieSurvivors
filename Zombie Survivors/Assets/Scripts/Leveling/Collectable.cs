using System.Collections;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
	protected abstract void ActionAfterCollected();

	protected IEnumerator Collect(Transform playerTransform, float collectTime)
	{
		float elapsedTime = 0;

		while (elapsedTime < collectTime)
		{
			transform.position = Vector3.Lerp(transform.position, playerTransform.position, (elapsedTime / collectTime));
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, (elapsedTime / collectTime));
			elapsedTime += Time.deltaTime;

			if (Vector3.Distance(transform.position, playerTransform.position) <= 0.5f)
			{
				ActionAfterCollected();
				Destroy(gameObject);
			}

			yield return null;
		}
	}
}
