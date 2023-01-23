using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScalingConfiguration_", menuName = "Scriptable Objects/Scaling Configuration")]
public class ScalingConfigurationSO : ScriptableObject
{
    public AnimationCurve HealthCurve;
    public AnimationCurve DamageCurve;
    public AnimationCurve SpeedCurve;
    public AnimationCurve SpawnRateCurve;
    public AnimationCurve SpawnCountCurve;
}
