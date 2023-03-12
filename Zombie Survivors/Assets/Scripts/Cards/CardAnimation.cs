using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
	[SerializeField] private float increaseAmmount = 1.05f;
	[SerializeField] private float animationDuration = 0.2f;

	public void PlayShowAnimation(RectTransform rectTransform, Action actionOnAnimationEnd)
	{
		rectTransform.DOScale(rectTransform.localScale * increaseAmmount, animationDuration).SetEase(Ease.Flash).OnComplete(() => {
			actionOnAnimationEnd();
		});
	}

	public void PlayCloseAnimation(RectTransform rectTransform, Action actionOnAnimationEnd)
	{
		rectTransform.DOScale(rectTransform.localScale, animationDuration).SetEase(Ease.InFlash).OnComplete(() => {
			actionOnAnimationEnd();
		});
	}
}
