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
    [Min(0)]
    public int OffspringPerCellBurst = 4; // A possible setting for how many viruses are emitted when an infected cell bursts. The header causes an error if there are no properties here.


    /// <summary>
    /// Called when this scriptable object is first created
    /// </summary>
    private void Awake()
    {
        Type = EnemyTypes.Virus_Base;
    }
}
