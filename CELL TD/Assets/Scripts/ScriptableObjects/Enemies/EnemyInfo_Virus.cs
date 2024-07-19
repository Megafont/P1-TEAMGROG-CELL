using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any virus-type enemy.
/// It inherits from the EnemyInfoBase scriptable object so that all properties common to all enemy types
/// are included automatically.
/// </summary>
[CreateAssetMenu(fileName = "New EnemyInfo_Virus", menuName = "Enemy Info Assets/New EnemyInfo_Virus Asset")]
public class EnemyInfo_Virus : EnemyInfo_Base
{
    [Header("Virus-Specific Stats")]

    // Add properties specific to and shared by all virus-type enemies only.
    [Tooltip("This sets how likely it is that a virus will convert a player unit into another virus.")]
    [Range(0f, 1f)]
    public float ChanceOfConversion = 0.25f;

    [Tooltip("How close the player unit must be to the virus for the virus to be able to convert it.")]
    [Min(0f)]
    public float ConversionRadius = 1f;

    [Tooltip("The virus prefab to use for spawning more viruses when a player unit is converted.")]
    public Transform VirusPrefab;



    /// <summary>
    /// Called when this scriptable object is first created
    /// </summary>
    private void Awake()
    {
        Type = EnemyTypes.Virus_Base;
    }
}
