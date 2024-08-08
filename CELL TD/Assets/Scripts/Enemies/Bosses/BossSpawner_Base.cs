using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;


/// <summary>
/// This is the class for bosses that spawn enemies
/// </summary>
/// <remarks>
/// NOTE: The boss appears to float above the ground because I set the Base Offset setting on the NavMesh component to 2.5f.
///       I tried just changing the position of the Armature and BOSS_Bacteria_Mesh child objects, but that didn't work.
///       So if this ever needs to be adjusted, that is what's making him levitate.
/// </remarks>
public class BossSpawner_Base : Enemy_Base    
{
    [Header("Reinforcements Settings")]

    [Tooltip("This sets which spawner the boss's reinforcements will come from. Set this value to -1 to make each enemy come from a random spawner. Setting it to -2 is an easy way to make reinforcements come from the same path as the boss itself. NOTE: Which lane the boss itself spawns from depends on which spawner you added it under in the WaveList asset for this level.")]
    [Min(-1)]
    [SerializeField]
    private int _ReinforcementsSpawnerIndex = -1;

    [Tooltip("Specifies the number of seconds that will elapse before the first reinforcements start spawning in.")]
    [Min(0)]
    [SerializeField]
    private float _FirstReinforcementsDelay = 2.0f;

    [Tooltip("Sets how many seconds will elapse between each reinforcement spawning in.")]
    [Min(0)]
    [SerializeField]
    private float _ReinforcementsSpawnFrequency = 1.0f;



    private Vector3 _BossStartPos;
    private Vector3 _StartPos;
    private List<EnemySpawner> _SpawnPoints;

    new void Awake()
    {
        base.Awake();

        _BossStartPos = transform.position;
        _SpawnPoints = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();

        if (_SpawnPoints.Count == 0)
        {
            throw new Exception("There are no enemy spawners on this level! The boss cannot spawn, and will not be able to spawn in reinforcements!");
        }
        else if (_ReinforcementsSpawnerIndex >= _SpawnPoints.Count)
        {
            Debug.LogWarning("The EnemySpawnIndex is set to a value that is higher than the last spawner's index! Reinforcements will spawn from random paths to avoid crashing.");
        }
    }

    new void Start()
    {
        base.Start();
        InvokeRepeating("SpawnEnemy", _FirstReinforcementsDelay, _ReinforcementsSpawnFrequency);
        // Do initialization here.
    }

    void SpawnEnemy()
    {       
        GameObject newEnemy = Instantiate(EnemyInfo_BossSpawner.spawnableEnemies[Random.Range(0, EnemyInfo_BossSpawner.spawnableEnemies.Length)]);

        if (_SpawnPoints.Count == 0 || _ReinforcementsSpawnerIndex == -2)
        {
            _StartPos = _BossStartPos;                
        }
        else if (_ReinforcementsSpawnerIndex == -1)
        {
            _StartPos = SelectRandomSpawnPos();
        }
        else
        {
            if (_ReinforcementsSpawnerIndex < _SpawnPoints.Count)
            {
                _StartPos = _SpawnPoints[_ReinforcementsSpawnerIndex].transform.position;
            }
            else
            {
                _StartPos = SelectRandomSpawnPos();
            }
        }

        newEnemy.transform.position = _StartPos;
        WaveManager.Instance.EnemyAdded();

        // Uncomment the following line to make it so enemies spawned by the boss don't award money to the player on death. This is pretty much the same line of code that is in Virus_Base.cs when a virus converts a macrophage into a new virus.
        //newEnemy.GetComponent<Enemy_Base>().GivesMoneyOnDeath = false; // Flag that this enemy should not give money to the player on death.
    }

    private Vector3 SelectRandomSpawnPos()
    {
        return _SpawnPoints[Random.Range(0, _SpawnPoints.Count)].transform.position;
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
