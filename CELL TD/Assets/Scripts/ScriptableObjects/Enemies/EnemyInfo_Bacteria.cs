using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any bacteria-type enemy.
/// It inherits from the EnemyInfoBase scriptable object so that all properties common to all enemy types
/// are included automatically.
/// </summary>
[CreateAssetMenu(fileName = "New EnemyInfo_Bacteria", menuName = "Enemy Info Assets/New EnemyInfo_Bacteria Asset")]
public class EnemyInfo_Bacteria : EnemyInfo_Base
{
    [Header("Bacteria-Specific Stats")]

    // Add properties specific to and shared by all bacteria-type enemies only.
    [Min(0)]
    public float OffspringPerMitosis = 2; // A possible setting for how many bacteria are created when one splits. The header causes an error if there are no properties here.




    /// <summary>
    /// Called when this scriptable object is first created
    /// </summary>
    private void Awake()
    {
        Type = EnemyTypes.Bacteria_Base;
    }
}
