using System;
using UnityEngine;

[DisallowMultipleComponent]
public class EvacuationArea : MonoBehaviour
{
    private Collider collider;

	private void Awake()
	{
		collider = GetComponent<Collider>();
		collider.enabled = false;
	}

	public void EnableCollider()
    {
		collider.enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
        if (other.transform.CompareTag("Player"))
        {
			GameManager.Instance.Evacuate(transform.position);
		}
	}
}
