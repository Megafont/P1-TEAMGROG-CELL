using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the class for bosses that spawn enemies
/// </summary>
/// 
public class BossSpawner_Base : Enemy_Base    
{
    private Vector3 _startPos;

    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
        _startPos = transform.position;
        InvokeRepeating("SpawnEnemy", 2.0f, 2.0f);
        // Do initialization here.
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(EnemyInfo_BossSpawner.spawnableEnemies[Random.Range(0,EnemyInfo_BossSpawner.spawnableEnemies.Length)]);
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


    public EnemyInfo_BossSpawner EnemyInfo_BossSpawner
    {
        get { return EnemyInfo as EnemyInfo_BossSpawner; }
    }
}
