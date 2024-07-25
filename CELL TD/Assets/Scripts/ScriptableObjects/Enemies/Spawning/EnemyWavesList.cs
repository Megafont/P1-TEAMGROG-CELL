using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Waves List", menuName = "Enemy Spawning Assets/New Waves List Asset")]
/// <summary>
/// This class basically holds all the data that tells the WaveManager in the level what to do for each wave.
/// </summary>
public class EnemyWavesList : ScriptableObject
{
    [Tooltip("Specifies a list of EnemyWaveInfo assets that tell the spawners what to spawn during each wave. Index 0 is wave 1, Index 1 is wave 2, etc.")]
    public List<EnemyWaveInfo> Waves = new List<EnemyWaveInfo>();



    public int Count { get { return Waves.Count; } }

    public EnemyWaveInfo this[int index]
    {
        get { return Waves[index]; } 
    }

}



