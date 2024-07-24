using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This class tracks how many cats are left across all spawners.
/// </summary>
public class WaveManager : MonoBehaviour
{
    public event EventHandler WaveEnded;
    public event EventHandler LevelCleared;
    public event EventHandler AnEnemyDied;
    public event EventHandler AnEnemyReachedGoal;



    [Tooltip("This is the EnemyWavesList scriptable object that tells the WaveManager what to spawn at each spawner during each wave in this level.")]
    [SerializeField]
    private EnemyWavesList _WavesList;

    [Tooltip("This list connects the spawn points in this level with the information in the WavesList asset specified above. For example, the spawner at index 0 will use the spawn data at index 0 in each wave in that data. The next one will use index 1 and so on.")]
    [SerializeField]
    private List<EnemySpawner> _EnemySpawners;



    public static WaveManager Instance;

    private int _TotalEnemiesInWave;
    private int _EnemiesRemainingInWave;
    
    private int _EnemiesKilled;
    private int _EnemiesReachedGoal;

    private int _TotalCatsDistracted;
    private int _TotalEnemiesReachedGoal;

    private float _SecondsSinceLevelStart;
    private float _SecondsSinceWaveStart;

    private int _CurrentWaveNumber = 0;
    private bool _WaveInProgress = false;

    private void Awake()
    {
    }

    private void InitInstance()
    {
        if (Instance != null)
        {
            Debug.LogError("There is already a WaveManager in this scene. Self destructing!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitInstance();
        Debug.Log(Instance.gameObject);

        Enemy_Base.OnEnemyDied += OnEnemyDied;
        Enemy_Base.OnEnemyReachedGoal += OnEnemyReachedGoal;      
    }

    private void Update()
    {
        _SecondsSinceLevelStart += Time.deltaTime;

        if (_WaveInProgress)
            _SecondsSinceWaveStart += Time.deltaTime;


        if (!_WaveInProgress && WaveNumber == _WavesList.Count && GameManager.Instance.HealthSystem.HealthAmount > 0)
        {
            LevelCleared?.Invoke(this, EventArgs.Empty);

            VictoryScreen.Show();
        }
    }

    private void OnDestroy()
    {
        Instance = null;

        Enemy_Base.OnEnemyDied -= OnEnemyDied;
        Enemy_Base.OnEnemyReachedGoal -= OnEnemyReachedGoal;
    }

    public void StartNextWave()
    {
        // Don't try to start a wave if one is already in progress.
        if (IsWaveInProgress)
            return;


        _WaveInProgress = true;


        _CurrentWaveNumber++;


        if (_WavesList[_CurrentWaveNumber - 1].EnemySpawnerInfos.Count <= 0)
        {
            Debug.LogError($"WaveManager can't spawn enemies because Wave {_CurrentWaveNumber} has no spawning data for any spawners!");
            return;
        }
        else if (AllSpawnersAreSetToSpawn0InCurrentWave())
        {
            Debug.LogError($"WaveManager can't spawn enemies because Wave {_CurrentWaveNumber} has spawning data for one or more spawners, but no enemies set to spawn from any of them!");
            return;
        }


        EnemyWaveInfo waveInfo = _WavesList[_CurrentWaveNumber - 1];
        for (int i = 0; i < _EnemySpawners.Count; i++)
        {
            EnemySpawner spawner = _EnemySpawners[i];
            if (spawner == null)
            {
                Debug.LogWarning("WaveManager has a null entry in its EnemySpawners list!  Skipping it...");
                continue;
            }

            // Does the EnemyWaveInfo object for this wave has some instructions for this spawner? If so, then activate it.
            if (i < waveInfo.EnemySpawnerInfos.Count)
            {
                if (waveInfo.EnemySpawnerInfos[i] == null)
                {
                    Debug.LogWarning($"WaveManager encountered a null entry in its EnemyWaveInfo object at EnemyWaveInfo.EnemySpawners[{i}]!  Skipping it...");
                    continue;
                }

                if (waveInfo.EnemySpawnerInfos[i].EnemySpawnGroups.Count > 0)
                    spawner.StartNextWave(waveInfo.EnemySpawnerInfos[i]);
            }
        }

        CalculateTotalEnemiesInWave();

        _EnemiesRemainingInWave = _TotalEnemiesInWave;
        _EnemiesKilled = 0;
        _EnemiesReachedGoal = 0;

        
        //HUD.ShowWaveDisplay();
        //HUD.UpdateWaveInfoDisplay(_WaveNumber, _CatsRemainingInWave);
    }

    /// <summary>
    /// This function checks if the current wave has no enemies set to spawn from any spawners.
    /// </summary>
    /// <returns>True if the current wave has no enemies set to spawn from any spawners.</returns>
    private bool AllSpawnersAreSetToSpawn0InCurrentWave()
    {
        foreach (EnemySpawnerInfo spawnerInfo in _WavesList[_CurrentWaveNumber - 1].EnemySpawnerInfos)
        {
            if (spawnerInfo.EnemySpawnGroups.Count > 0)
            {
                foreach (EnemySpawnGroupInfo group in spawnerInfo.EnemySpawnGroups)
                {
                    if (group.NumberToSpawn > 0)
                    {
                        return false;
                    }

                } // end foreach EnemySpawnGroupInfo
            }

        } // end foreach EnemySpawnerInfo


        return true;
    }

    public void StopAllSpawning()
    {
        foreach (EnemySpawner spawner in _EnemySpawners)
        {
            spawner.StopSpawner();
        }
    }

    public void EnemyAdded()
    {
        _EnemiesRemainingInWave++;
        _TotalEnemiesInWave++;
    }

    public void OnEnemyDied(object Sender, EventArgs e)
    {
        _EnemiesRemainingInWave--;
        _EnemiesKilled++;
        _TotalCatsDistracted++;

        AnEnemyDied?.Invoke(Sender, EventArgs.Empty);
        //HUD.UpdateWaveInfoDisplay(_WaveNumber, _CatsRemainingInWave);

        if (_EnemiesRemainingInWave < 1)
        {
            //HUD.HideWaveDisplay();
            _WaveInProgress = false;

            WaveEnded?.Invoke(this, EventArgs.Empty);

            

            OnWaveEnded(this, EventArgs.Empty);


            if (_CurrentWaveNumber >= _WavesList.Count /* && player.IsDead */)
            {
                //HUD.RevealVictory();
            }
        }
        
    }

    public void OnEnemyReachedGoal(object Sender, EventArgs e)
    {
        _EnemiesRemainingInWave--;
        _EnemiesReachedGoal++;
        _TotalEnemiesReachedGoal++;

        AnEnemyReachedGoal?.Invoke(Sender, EventArgs.Empty);
        //HUD.UpdateWaveInfoDisplay(_WaveNumber, _CatsRemainingInWave);

        if (_EnemiesRemainingInWave < 1)
        {
            //HUD.HideWaveDisplay();
            _WaveInProgress = false;

            WaveEnded?.Invoke(this, EventArgs.Empty);
            OnWaveEnded(this, EventArgs.Empty);

            if (_CurrentWaveNumber >= _WavesList.Count)
            {
                //HUD.RevealVictory();
            }
        }
    }


    private void CalculateTotalEnemiesInWave()
    {
        _TotalEnemiesInWave = 0;
        foreach (EnemySpawnerInfo spawnerInfo in _WavesList[_CurrentWaveNumber - 1].EnemySpawnerInfos)
        {
            foreach (EnemySpawnGroupInfo group in spawnerInfo.EnemySpawnGroups)
            {
                _TotalEnemiesInWave += group.NumberToSpawn;
            }
        }
    }

    private void OnWaveEnded(object sender, EventArgs e)
    {
        GameManager.Instance.MoneySystem.AddCurrency(_WavesList[_CurrentWaveNumber - 1].WaveReward);
 
        NextWave.Instance.EnableButton();
    }



    public int TotalWavesInLevel { get { return _WavesList.Count; } }

    public int WaveNumber { get { return _CurrentWaveNumber; } }
    public bool IsWaveInProgress { get { return _WaveInProgress; } }

    public int NumEnemiesKilledInWave { get { return _EnemiesKilled; } }
    public int NumEnemiesReachedGoalInWave { get { return _EnemiesReachedGoal; } }
    public int TotalEnemiesInWave { get { return _TotalEnemiesInWave; } }
   
    public int TotalEnemiesKilledInLevel { get { return _TotalCatsDistracted; } }
    public int TotalEnemiesReachedGoalInLevel { get { return _TotalEnemiesReachedGoal; } }

    public float SecondsElapsedSinceLevelStarted { get { return _SecondsSinceLevelStart; } }
    public float SecondsElapsedSinceWaveStarted { get { return _SecondsSinceWaveStart; } }

}

