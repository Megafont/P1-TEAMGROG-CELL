using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Waves List (OLD)", menuName = "Enemy Spawning Assets/New Waves List Asset (OLD)")]
/// <summary>
/// This class basically holds all the data that tells the WaveManager in the level what to do for each wave.
/// </summary>
public class EnemyWavesList_OLD : ScriptableObject
{
    [Tooltip("Specifies a list of EnemyWaveInfo assets that tell a spawner what to spawn. You can leave an elemnt in the list set to null or set the enemy amount to 0 to make the spawner do nothing on that particular wave.")]
    public List<EnemyWaveInfo_OLD> Waves = new List<EnemyWaveInfo_OLD>();




    public int Count { get { return Waves.Count; } }

    public EnemyWaveInfo_OLD this[int index]
    {
        get { return Waves[index]; } 
    }

}



