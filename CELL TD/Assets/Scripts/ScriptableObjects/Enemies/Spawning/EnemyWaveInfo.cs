using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This class stores the settings for a single wave.
/// </summary>
[Serializable]
//[CreateAssetMenu(fileName = "New Wave Info", menuName = "Enemy Spawning Assets/New WaveInfo Asset")]
public class EnemyWaveInfo //: ScriptableObject
{
    [Tooltip("This is how much currency the player is awarded for surviving this wave.")]
    [Min(0)]
    public int WaveReward = 100;

    [Tooltip("This list should have one entry for each spawner in the level. It basically tells each spawner in the level what to spawn in each wave. The spawner associated with each element is set in the level's WaveManager object. Index 0 = wave 1, index 1 = wave 2, and so on.")]
    public List<EnemySpawnerInfo> EnemySpawnerInfos;



    public int Count { get { return EnemySpawnerInfos.Count; } }

    public EnemySpawnerInfo this[int index]
    {
        get { return EnemySpawnerInfos[index]; }
    }
}

[Serializable]
public class EnemySpawnerInfo
{
    [Tooltip("This specifies the start delay (in seconds). You can use it to make this spawner wait a bit before it begins to spawn enemies rather than having all spawn points start immediately when the wave starts.")]
    [Min(0)]
    public float StartDelay = 0f;

    [Tooltip("This list specifies the enemy groups that this spawner should spawn. They will occur one after the other in the current wave. Leave this list empty, or add an EnemySpawnGroup below with its NumberToSpawn option set to 0 to have this spawner be inactive during this wave.")]
    public List<EnemySpawnGroupInfo> EnemySpawnGroups;


    public int Count { get { return EnemySpawnGroups.Count; } }

    public EnemySpawnGroupInfo this[int index]
    {
        get { return EnemySpawnGroups[index]; }
    }
}