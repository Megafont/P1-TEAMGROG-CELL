using Assets.Scripts.Towers;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your tower.
/// </summary>
[CreateAssetMenu(fileName = "New TowerInfo_Lymphocyte_SlowingAOETower", menuName = "Tower Info Assets/New TowerInfo_Lymphocyte_SlowingAOETower Asset")]
public class TowerInfo_Lymphocyte_SlowingAOETower : TowerInfo_Base
{
    [Header("Lymphocyte (SlowingAOETower)-Specific Stats")]

    [Tooltip("The status effect applied by this tower.")]
    [SerializeField]
    public StatusEffectInfo_SlowedMovement StatusEffect;

    [Tooltip("The status effect applied by this tower.")]
    [SerializeField]
    public StatusEffectInfo_SlowedMovement StatusEffect1;

    [Tooltip("The status effect applied by this tower.")]
    [SerializeField]
    public StatusEffectInfo_SlowedMovement StatusEffect2;

    [Tooltip("The status effect applied by this tower.")]
    [SerializeField]
    public StatusEffectInfo_SlowedMovement StatusEffect3;
}

