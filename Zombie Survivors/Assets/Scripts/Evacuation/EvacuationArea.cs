using DG.Tweening;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EvacuationArea : MonoBehaviour
{
	[SerializeField] private Transform helicopter;
    private Collider areaCollider;

	private void Awake()
	{
		areaCollider = transform.GetComponent<Collider>();
		areaCollider.enabled = false;
	}
	private void OnEnable()
	{
		AudioManager.Instance.PlayMusicWithFade(SoundTitle.Helicopter_Arrive);

		StaticEvents.OnComradeBoarded += StaticEvents_OnComradeBoarded;
	}

	private void OnDisable()
	{
		StaticEvents.OnComradeBoarded -= StaticEvents_OnComradeBoarded;
	}

	private void StaticEvents_OnComradeBoarded()
	{
		StartCoroutine(AnimateHelicopterScale());
	}

	private IEnumerator AnimateHelicopterScale()
	{
		Vector3 initialScale = helicopter.localScale;
		Vector3 scaleTo = initialScale * 1.2f;

		yield return helicopter.DOScale(scaleTo, 0.1f).OnComplete(() => helicopter.DOScale(initialScale, 0.1f));
	}

	public void EnableCollider()
    {
		areaCollider.enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.transform.CompareTag("Player"))
        {
			GameManager.Instance.Evacuate(transform);
		}
	}
}
