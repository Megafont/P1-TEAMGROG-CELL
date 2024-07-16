using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define a status effect.
/// </summary>
[CreateAssetMenu(fileName = "New StatusEffectInfo_Base", menuName = "Status Effect Info Assets/New StatusEffectInfo_Base Asset")]
public class StatusEffectInfo_Base : ScriptableObject
{
    [Tooltip("The display name of this status effect")]
    public string DisplayName;
    [Tooltip("The description of this status effect")]
    public string Description;
    [Tooltip("The type of this status effect")]
    public StatusEffectTypes Type;
    [Tooltip("The icon of this status effect")]
    public Sprite Icon;
    [Tooltip("The duration of this status effect")]
    public float Duration;
}
