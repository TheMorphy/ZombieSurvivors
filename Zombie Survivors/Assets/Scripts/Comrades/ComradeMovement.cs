using UnityEngine;

[DisallowMultipleComponent]
public class ComradeMovement : MonoBehaviour
{
	private Comrade comrade;
    [SerializeField] private Transform bodyPivot;

	private void Awake()
	{
		comrade = GetComponent<Comrade>();
	}

	private void Update()
	{
		if(comrade.GetPlayer() != null)
			bodyPivot.rotation = comrade.GetPlayer().playerController.GetRotation();
	}
}
