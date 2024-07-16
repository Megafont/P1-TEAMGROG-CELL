using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any fungi-type enemy.
/// It inherits from the EnemyInfoBase scriptable object so that all properties common to all enemy types
/// are included automatically.
/// </summary>
[CreateAssetMenu(fileName = "New EnemyInfo_Fungi", menuName = "Enemy Info Assets/New EnemyInfo_Fungi Asset")]
public class EnemyInfo_Fungi : EnemyInfo_Base
{
    [Header("Fungi-Specific Stats")]

    // Add properties specific to and shared by all fungi-type enemies only.
    [Min(0)]
    public int SporesPerBurst = 3; // A possible setting for how many spores are released per burst. The header causes an error if there are no properties here.



    /// <summary>
    /// Called when this scriptable object is first created
    /// </summary>
    private void Awake()
    {
        Type = EnemyTypes.Fungi_Base;
    }
}