using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[Serializable]
public class EnemySpawnGroupInfo
{
    [Tooltip("Set what type of enemy to spawn. Multiple of the same enemy type can be used in a wave.")]
    public EnemyInfo_Base EnemyInfo;

    [Tooltip("Set the amount of this enemy to spawn")]
    [Min(1)]
    public int NumberToSpawn = 10;

    [Tooltip("Sets the time between each enemy spawn (in seconds).")]
    [Min(0.25f)]
    public float TimeBetweenSpawns = 0.25f;
}