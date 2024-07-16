using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Enemy Spawning Assets/New Wave Asset")]
public class Waves : ScriptableObject
{

    public List<Wave> WaveInfo = new List<Wave>();

}

[System.Serializable]
public class Wave
{
    public List<SpawnInfo> Enemies = new List<SpawnInfo>();
    public int WaveReward = 100;
}

[System.Serializable]
public class SpawnInfo
{
    public EnemyInfo_Base Enemy;
    public int amount = 0;
    public float timeBetween = 0.0f;
}