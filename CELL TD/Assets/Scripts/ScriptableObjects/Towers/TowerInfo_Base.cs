using Assets.Scripts.Towers;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your tower.
/// </summary>
[CreateAssetMenu(fileName = "New TowerInfo_Base", menuName = "Tower Info Assets/New TowerInfo_Base Asset")]
public class TowerInfo_Base : ScriptableObject
{
    [Header("General Tower Info")]

    public string DisplayName; // The name displayed for this tower in the UI

    [TextArea(3, 10)]
    public string Description; // The description displayed in the UI

    public TowerTypes TowerType; // The type of this tower.
    public Sprite DisplayIcon; // The icon used for this tower in the UI
    public GameObject Prefab; // The prefab for this tower type
    public TargetingTypes TargetingType; // The targeting type of this tower
    public List<EnemyTypes> TargetedEnemyTypes; // The types of enemies this tower affects


    [Header("Tower Stats")]

    [Tooltip("How much it costs the player to build this type of tower.")]
    [SerializeField, Min(0)]
    public float BuildCost;

    [Tooltip("This is the percentage of the cost that is refunded when the player destroys the tower.")]
    [Range(0f, 1f)]
    [SerializeField]
    public float BaseRefundPercentage = 0.85f;

    [Tooltip("The starting max health of this tower.")]
    [SerializeField, Min(0)]
    public float BaseMaxHealth = 100f;

    [Tooltip("The base damage done by this tower if it has direct attacks.")]
    [SerializeField, Min(0)]
    public float BaseDamageValue;

    [Tooltip("The base fire rate of this tower if it has direct attacks.")]
    [SerializeField, Min(0)]
    public float BaseFireRate;

    [Tooltip("The range of this tower.")]
    [SerializeField, Min(0)]
    public float BaseRange = 3f;


    [Tooltip("How many targets a level 1 instance of this tower can have at one time.")]
    [SerializeField, Min(0)]
    public int BaseNumberOfTargets;

    [Header("Tower Upgrades")]

    [Tooltip("Defines the upgrades for this tower type. The first item in the list represents the first level up, the next one the 2nd, and so on.")]
    public List<TowerUpgradeDefinition> LevelUpDefinitions;
}

[Serializable]
public class TowerUpgradeDefinition
{
    [Tooltip("How much this upgrade costs the player.")]
    public float UpgradeCost;

    [Space(10)]

    [Tooltip("This list defines which stats get upgraded by this level up for this tower type, and by how much.")]
    public List<TowerStatUpgradeDefinition> StatUpgradeDefinitions;
}

[Serializable]
public class TowerStatUpgradeDefinition
{
    [Tooltip("The stat to upgrade.")]
    public TowerStats TowerStat;

    [Tooltip("How much this stat increases by.")]
    public float UpgradeAmount;
}
