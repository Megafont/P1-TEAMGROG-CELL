using Assets.Scripts.Towers;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your tower.
/// </summary>
[CreateAssetMenu(fileName = "New TowerInfo_Neutrophil_ProjectileTower", menuName = "Tower Info Assets/New TowerInfo_Neutrophil_ProjectileTower Asset")]
public class TowerInfo_Neutrophil_ProjectileTower : TowerInfo_Base
{
    [Header("Neutrophil (Projectile Tower)-Specific Stats")]

    // A dummy setting. The header causes an error if there are no properties here.
    [Tooltip("Dummy Stat")]
    [SerializeField]
    public int DummyStat;

}

