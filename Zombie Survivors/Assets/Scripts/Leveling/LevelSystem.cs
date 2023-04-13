using System;

public class LevelSystem
{
    private int level;
	private int experience;
	private int experienceToNextLevel;

    public event EventHandler OnExperienceChanged;
	public event EventHandler OnLevelUp;

	public bool LeveledUp = false;

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
	}

    public void AddExperience(int expIncreasePercentage)
    {
        if (LeveledUp) return;

        experience += expIncreasePercentage;

        // Enough to level up
        if (experience >= experienceToNextLevel)
        {
            LeveledUp = true;

            level++;
            experience -= experienceToNextLevel;

			OnLevelUp?.Invoke(this, EventArgs.Empty);
		}

		OnExperienceChanged?.Invoke(this, EventArgs.Empty);
	}

    public int GetPlayerLevel()
    {
        return level;
    }
    public int GetPlayerExperience()
    {
        return experience;
    }

    public int GetPlayerExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }

    public float GetPlayerExperienceNormalized()
    {
        return (float)experience/ experienceToNextLevel;
    }
}
