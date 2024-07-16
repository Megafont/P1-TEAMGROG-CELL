using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the class for bosses that spawn enemies
/// </summary>
/// 
public class BossSpawner_Base : Enemy_Base    
{
    private EnemyInfo_BossSpawner _infoRef;
    private Vector3 _startPos;

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
        _infoRef = (EnemyInfo_BossSpawner)_EnemyInfo;
        _startPos = transform.position;
        InvokeRepeating("SpawnEnemy", 2.0f, 2.0f);
        // Do initialization here.
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(_infoRef.spawnableEnemies[Random.Range(0,_infoRef.spawnableEnemies.Length)]);
        newEnemy.transform.position = _startPos;
        WaveManager.Instance.EnemyAdded();
    }

    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();
    }

    /// <summary>
    /// Initializes the state machine of this enemy.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitStateMachine()
    {
        // This probably isn't needed.
        //base.InitStateMachine();
    }
}
