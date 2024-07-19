using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class EnemySpawnInfo2
{
    [Tooltip("Set what type of enemy to spawn. Multiple of the same enemy type can be used in a wave.")]
    public EnemyInfo_Base EnemyInfo;

    [Tooltip("Set the amount of this enemy to spawn")]
    public int NumberToSpawn = 10;

    [Tooltip("Sets the time between each enemy spawn (in seconds).")]
    public float TimeBetweenSpawns = 2.0f;

}