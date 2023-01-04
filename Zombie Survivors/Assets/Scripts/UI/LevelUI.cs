using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNumberText;
	[SerializeField] private Image experienceBarImage;
	private LevelSystem levelSystem;

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
