using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Waves List", menuName = "Enemy Spawning Assets/New Waves List Asset")]
public class EnemyWavesList : ScriptableObject
{
    [Tooltip("Specifies a list of EnemyWaveInfo assets that tell a spawner what to spawn. You can leave an elemnt in the list set to null or set the enemy amount to 0 to make the spawner do nothing on that particular wave.")]
    public List<EnemyWaveInfo> Waves = new List<EnemyWaveInfo>();




    public int Count { get { return Waves.Count; } }

    public EnemyWaveInfo this[int index]
    {
        get { return Waves[index]; } 
    }

}



