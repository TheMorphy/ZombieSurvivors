using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelSystem
{
    [SerializeField] private int level;
	[SerializeField] private int experience;
	[SerializeField] private int experienceToNextLevel;

    public event EventHandler OnExperienceChanged;
	public event EventHandler OnLevelChanged;

    public LevelSystem()
    {
        level= 0;
        experience= 0;
        experienceToNextLevel= 100;
	}

    public void AddExperience(int expIncreasePercentage)
    {
        experience += expIncreasePercentage;

        // Enough to level up
        while (experience >= experienceToNextLevel)
        {
            level++;
            experience -= experienceToNextLevel;

			OnLevelChanged?.Invoke(this, EventArgs.Empty);
		}

        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
	}

    public int GetPlayerLevel()
    {
        return level;
    }

    public float GetPlayerExperienceNormalized()
    {
        return (float)experience/ experienceToNextLevel;
    }
}
