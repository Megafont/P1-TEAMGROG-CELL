using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySpawnInfo", menuName = "Enemy Spawning Assets/New EnemySpawnInfo Asset")]
public class EnemySpawnInfo : ScriptableObject
{
    [Tooltip("Set what type of enemy to spawn. Multiple of the same enemy type can be used in a wave.")]
    public EnemyInfo_Base EnemyInfo;

    [Tooltip("Set the amount of this enemy to spawn")]
    public int NumberToSpawn;

}