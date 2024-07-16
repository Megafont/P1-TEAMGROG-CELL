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
    [Tooltip("A list of Wave scriptable Objects that have a list of enemies to spawn")]
    private List<Waves> _Waves;

    [Tooltip("The time between enemies spawning")]
    [SerializeField]
    [Min(1f)]
    private float _TimeBetweenSpawns;

    [Header("Game Object References")]
    [SerializeField, Tooltip("One possible spawn point for enemies")]
    private Transform _SpawnPoint1;


    private int _CurrentWave;

    private int _CurrentWaveInfo = 0;

    private Transform _SpawnedEnemiesParent;


    private void Awake()
    {
        _Waves = Resources.LoadAll<Waves>("").ToList();
        _CurrentWave = 0;
        _SpawnedEnemiesParent = GameObject.Find("Spawned Enemies Parent").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNextWave()
    {
        StartCoroutine(Spawn(_CurrentWave++));
    }
    public void StopSpawner()
    {
        StartCoroutine(Spawn(_CurrentWave));
    }
    IEnumerator Spawn(int currentWave)
    {
        int currentEnemyType = 0;
        EnemyTypes type = _Waves[0].WaveInfo[_CurrentWaveInfo].Enemies[currentEnemyType].Enemy.Type;
        int enemiesOfCurrentType = _Waves[_CurrentWaveInfo].WaveInfo[currentWave].Enemies[currentEnemyType].amount;
        int totalEnemies = EnemiesInCurrentWave();
        GameObject enemy = null;
        
        for (int i = 0; i < totalEnemies; i++)
        {
            if( enemiesOfCurrentType == 0 ) {
                type = _Waves[_CurrentWaveInfo].WaveInfo[currentWave].Enemies[currentEnemyType].Enemy.Type;
                enemiesOfCurrentType = _Waves[_CurrentWaveInfo].WaveInfo[currentWave].Enemies[currentEnemyType].amount;
            }

            enemy = Instantiate(_Waves[_CurrentWaveInfo].WaveInfo[currentWave].Enemies[currentEnemyType].Enemy.Prefab, _SpawnedEnemiesParent);
            float delayBetween = _Waves[_CurrentWaveInfo].WaveInfo[currentWave].Enemies[currentEnemyType].timeBetween;


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
        Wave current = _Waves[_CurrentWaveInfo].WaveInfo[_CurrentWave - 1];
        int tally = 0;
        foreach(SpawnInfo enemy in current.Enemies)
        {
            tally += enemy.amount;
        }
        return tally;
    }

    public int WaveReward()
    {
        Wave current = _Waves[_CurrentWaveInfo].WaveInfo[_CurrentWave - 1];
        return current.WaveReward;
    }

    public int NumberOfWaves { get { return _Waves.Count; } }
}
