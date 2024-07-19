using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This class stores the settings for a single wave.
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "New Wave Info", menuName = "Enemy Spawning Assets/New WaveInfo Asset")]
public class EnemyWaveInfo : ScriptableObject
{
    [Tooltip("This specifies the start delay (in seconds). You can use it to make one spawn point wait a bit before it begins to spawn enemies rather than having all spawn points start immediately when the wave starts.")]
    public float StartDelay = 0f;

    [Tooltip("This is how much currency the player is awarded for surviving this wave.")]
    public int WaveReward = 100;


    [Tooltip("Specifies the list of enemies that will spawn during this wave. The same enemy type can be added more than once, for example to have a second group of that enemy come in that spawns faster than the first.")]
    public List<EnemySpawnInfo2> Enemies = new List<EnemySpawnInfo2>();
}

