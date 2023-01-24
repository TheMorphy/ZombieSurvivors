using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField]
    private Image bossHealth;

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void UpdateBossHealth(float percentage)
    {
        bossHealth.fillAmount = percentage;
	}

	public Image GetHealthbar()
	{
		return bossHealth;
	}

	public Transform GetUITransform()
	{
		return transform;
	}
}
