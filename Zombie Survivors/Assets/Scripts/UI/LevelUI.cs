using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelUI : MonoBehaviour
{
	[SerializeField] private GameObject[] upgradeCards;

	private LevelUIAnimated levelUIAnimated;

    [SerializeField] private TextMeshProUGUI levelNumberText;
	[SerializeField] private Image experienceBarImage;

	private LevelSystem levelSystem;

	private void Awake()
	{
		levelUIAnimated = GetComponent<LevelUIAnimated>();
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void InitializeUpgradeCards()
	{
		for (int i = 0; i < upgradeCards.Length; i++)
		{
			int rand = UnityEngine.Random.Range(0, 2);

			switch (rand)
			{
				case 0:

					break;
				case 1:

					break;
			}
		}
	}

	private void SetExperienceBarSize(float experienceNormalized)
	{
		experienceBarImage.fillAmount = experienceNormalized;
	}

	private void SetLevelNumber(int levelNumber)
	{
		levelNumberText.text = (levelNumber + 1).ToString();
	}

	public void SetLevelSystem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;

		SetLevelNumber(levelSystem.GetPlayerLevel());
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
	}

	private void LevelSystem_OnLevelChanged(object sender, EventArgs eventArgs)
	{
		SetLevelNumber(levelSystem.GetPlayerLevel());
	}

	private void LevelSystem_OnExperienceChanged(object sender, EventArgs eventArgs)
	{
		SetExperienceBarSize(levelSystem.GetPlayerExperienceNormalized());
	}
}
