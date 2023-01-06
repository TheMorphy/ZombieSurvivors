using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIAnimated : MonoBehaviour
{
    private LevelSystem levelSystem;
    private bool IsAnimating;

	private int level;
	private int experience;
	private int experienceToNextLevel;

	public LevelUIAnimated(LevelSystem levelSystem)
	{
		SetLevelSsytem(levelSystem);
	}

	private void SetLevelSsytem(LevelSystem levelSystem)
	{
		this.levelSystem = levelSystem;

		level = levelSystem.GetPlayerLevel();
		experience = levelSystem.GetPlayerExperience();
		experienceToNextLevel = levelSystem.GetPlayerExperienceToNextLevel();

		levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
		levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
	}

	private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
	{
		IsAnimating = true;
	}

	private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
	{
		IsAnimating = true;
	}

	private void Update()
	{
		if (IsAnimating)
		{
			if(level < levelSystem.GetPlayerLevel())
			{
				AddExperience();
			}
			else
			{
				if(experience < levelSystem.GetPlayerExperience())
				{
					AddExperience();
				}
				else
				{
					IsAnimating = false;
				}
			}
		}
	}

	private void AddExperience()
	{
		experience++;

		if(experience >= experienceToNextLevel)
		{
			level++;
			experience = 0;
		}
	}
}
