using DG.Tweening;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
	public float bounceHeight = 0.2f; // The height of the bounce
	public float bounceDuration = 0.5f; // The duration of the bounce animation
	public AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // The curve of the bounce animation

	private Vector3 originalScale; // The original scale of the object
	private bool isExpanded = false; // Whether the object is currently expanded

	void Start()
	{
		originalScale = transform.localScale;
	}

	public void PlayAnimation()
	{
		if (isExpanded)
		{
			// If the object is already expanded, return it to its original scale
			transform.DOScale(originalScale, bounceDuration)
				.SetEase(bounceCurve);
			isExpanded = false;
		}
		else
		{
			// If the object is not expanded, expand it and set the flag to true
			transform.DOKill(); // Stop any previous tweens
			transform.DOPunchScale(new Vector3(bounceHeight, bounceHeight, bounceHeight), bounceDuration, 0, 1f)
				.SetEase(bounceCurve);
			isExpanded = true;
		}
	}
}
