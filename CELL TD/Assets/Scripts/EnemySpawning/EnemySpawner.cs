using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;


public class EnemySpawner : MonoBehaviour
{
    private EnemySpawnerInfo _EnemySpawningInfo;



    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNextWave(EnemySpawnerInfo enemySpawningInfo)
    {
        // If we received a null value, then simply return.
        if (enemySpawningInfo == null)
            throw new ArgumentNullException(nameof(enemySpawningInfo));


        _EnemySpawningInfo = enemySpawningInfo;

        StartCoroutine(Spawn());
    }
    public void StopSpawner()
    {
        StopCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        // Wait until the initial delay expires before this spawner begins spawning.
        yield return new WaitForSeconds(_EnemySpawningInfo.StartDelay);


        int currentEnemyType = 0;

        int enemiesOfCurrentType = _EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].NumberToSpawn;
        int totalEnemies = EnemiesInCurrentWave();
        GameObject enemy = null;

        for (int i = 0; i < totalEnemies; i++)
        {
            if (_EnemySpawningInfo.EnemySpawnGroups.Count <= 0)
            {
                Debug.LogWarning("EnemySpawner received an EnemySpawnGroup that is set to spawn 0 enemies! To make the spawner spawn nothing in a given wave, simply set the EnemyGroupInfos list to have 0 elements for that spawner in that wave. Skipping this enemy spawn group.");
                continue;
            }
            else if (_EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].EnemyInfo == null)
            {
                Debug.LogWarning("EnemySpawner received an EnemySpawnGroup that has a null value for the enemy type! Skipping it.");
                continue;
            }

            EnemyTypes type = _EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].EnemyInfo.Type;

            if ( enemiesOfCurrentType == 0 ) {
                type = _EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].EnemyInfo.Type;
                enemiesOfCurrentType = _EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].NumberToSpawn;
            }

            enemy = Instantiate(_EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].EnemyInfo.Prefab, transform);

            float delayBetween = _EnemySpawningInfo.EnemySpawnGroups[currentEnemyType].TimeBetweenSpawns;


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
        int tally = 0;
        foreach(EnemySpawnGroupInfo group in _EnemySpawningInfo.EnemySpawnGroups)
        {
            tally += group.NumberToSpawn;
        }
        return tally;
    }

}
