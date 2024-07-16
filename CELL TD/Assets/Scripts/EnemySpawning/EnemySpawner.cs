using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [Tooltip("An EnemyWavesList scriptable Object that contains a list of enemies to spawn in each wave.")]
    [SerializeField]
    private EnemyWavesList _WavesList;


    private int _CurrentWaveIndex;

    private Transform _SpawnedEnemiesParent;


    private void Awake()
    {
        _CurrentWaveIndex = -1;

        //_SpawnedEnemiesParent = GameObject.Find("Spawned Enemies Parent").transform;
        _SpawnedEnemiesParent = this.transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNextWave()
    {
        StartCoroutine(Spawn(_CurrentWaveIndex));
    }
    public void StopSpawner()
    {
        StartCoroutine(Spawn(_CurrentWaveIndex));
    }
    IEnumerator Spawn(int currentWave)
    {
        _CurrentWaveIndex++;

        yield return new WaitForSeconds(_WavesList[_CurrentWaveIndex].StartDelay);


        int currentEnemyType = 0;
        EnemyTypes type = _WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].EnemyInfo.Type;
        int enemiesOfCurrentType = _WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].NumberToSpawn;
        int totalEnemies = EnemiesInCurrentWave();
        GameObject enemy = null;
        
        for (int i = 0; i < totalEnemies; i++)
        {
            if( enemiesOfCurrentType == 0 ) {
                type = _WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].EnemyInfo.Type;
                enemiesOfCurrentType = _WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].NumberToSpawn;
            }

            enemy = Instantiate(_WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].EnemyInfo.Prefab, _SpawnedEnemiesParent);
            float delayBetween = _WavesList[_CurrentWaveIndex].Enemies[currentEnemyType].TimeBetweenSpawns;


            enemiesOfCurrentType--;
            if(enemiesOfCurrentType == 0)
            {
                currentEnemyType++;
            }

            yield return new WaitForSeconds(delayBetween);
        }

    }


    public int EnemiesInCurrentWave()
    {
        EnemyWaveInfo current = _WavesList[_CurrentWaveIndex];
        int tally = 0;
        foreach(EnemySpawnInfo enemy in current.Enemies)
        {
            tally += enemy.NumberToSpawn;
        }
        return tally;
    }

    public int WaveReward()
    {
        EnemyWaveInfo currentWave = _WavesList.Waves[_CurrentWaveIndex];
        return currentWave.WaveReward;
    }

    public int NumberOfWaves { get { return _WavesList.Count; } }
}
