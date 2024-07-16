using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any fungi-type enemy.
/// It inherits from the EnemyInfoBase scriptable object so that all properties common to all enemy types
/// are included automatically.
/// </summary>
[CreateAssetMenu(fileName = "New EnemyInfo_BossSpawner", menuName = "Enemy Info Assets/New EnemyInfo_BossSpawner Asset")]
public class EnemyInfo_BossSpawner : EnemyInfo_Base
{
    [Header("BossSpawner-Specific Stats")]

    // Enemies that can spawn while the boss is alive.
    public GameObject[] spawnableEnemies = new GameObject[0];

    /// <summary>
    /// Called when this scriptable object is first created
    /// </summary>
    private void Awake()
    {
        Type = EnemyTypes.Boss_Base;
    }
}