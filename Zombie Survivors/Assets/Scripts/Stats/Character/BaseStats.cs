using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats_", menuName = "Scriptable Objects/Base Stats/Base Stats")]
public class BaseStats : ScriptableObject
{
    public Dictionary<Stat, float> CharacterStats;
}

public enum Stat
{
    Health,
    MoveSpeed,
}
