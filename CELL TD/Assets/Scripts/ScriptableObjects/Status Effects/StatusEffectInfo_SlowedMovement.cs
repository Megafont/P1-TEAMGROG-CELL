using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define a status effect.
/// </summary>
[CreateAssetMenu(fileName = "New StatusEffectInfo_SlowedMovement", menuName = "Status Effect Info Assets/New StatusEffectInfo_SlowedMovement Asset")]
public class StatusEffectInfo_SlowedMovement : StatusEffectInfo_Base
{   
    [Tooltip("This sets how much the target will be slowed down. For example, value of 0.25 means that this status effect will limit the target to 25% of its normal speed.")]
    [Range(0f, 1f)]
    public float SlowdownPercentage = 0.5f;
}
