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
    


    public static WaveManager Instance;

    private int _TotalWavesInLevel = 10; //TODO: Change Later
    private int _WaveReward = 0;

    private List<EnemySpawner> _EnemySpawners;
    private int _TotalEnemiesInWave;
    private int _EnemiesRemainingInWave;
    
    private int _EnemiesKilled;
    private int _EnemiesReachedGoal;

    private int _TotalCatsDistracted;
    private int _TotalEnemiesReachedGoal;

    private float _SecondsSinceLevelStart;
    private float _SecondsSinceWaveStart;

    private int _WaveNumber = 0;
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

        _EnemySpawners = new List<EnemySpawner>();
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Enemy Spawner");

        foreach(GameObject spawner in spawners)
        {
            EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
            if(spawner.GetComponent<EnemySpawner>().NumberOfWaves > _TotalWavesInLevel)
            {
                //_TotalWavesInLevel = spawner.GetComponent<EnemySpawner>().NumberOfWaves;
                _EnemySpawners.Add(spawner.GetComponent<EnemySpawner>());
            }
            else
            {
                Debug.LogError($"Enemy spawner object \"{spawner.name}\" does not have an EnemySpawner component on it!");
            }
        }

    }

    private void Update()
    {
        _SecondsSinceLevelStart += Time.deltaTime;

        if (_WaveInProgress)
            _SecondsSinceWaveStart += Time.deltaTime;


        if (!_WaveInProgress && WaveNumber == _TotalWavesInLevel && GameManager.Instance.HealthSystem.HealthAmount > 0)
        {
            LevelCleared?.Invoke(this, EventArgs.Empty);

            VictoryScreen.Show();
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
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

        _WaveNumber++;
        

        FindAllSpawners();

        foreach (EnemySpawner spawner in _EnemySpawners)
        {
            spawner.StartNextWave();
        }

        CalculateTotalEnemiesInWave();
        CalculateWaveReward();

        _EnemiesRemainingInWave = _TotalEnemiesInWave;
        _EnemiesKilled = 0;
        _EnemiesReachedGoal = 0;

        
        //HUD.ShowWaveDisplay();
        //HUD.UpdateWaveInfoDisplay(_WaveNumber, _CatsRemainingInWave);
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

        Debug.Log(_EnemiesRemainingInWave);

        AnEnemyDied?.Invoke(Sender, EventArgs.Empty);
        //HUD.UpdateWaveInfoDisplay(_WaveNumber, _CatsRemainingInWave);

        if (_EnemiesRemainingInWave < 1)
        {
            //HUD.HideWaveDisplay();
            _WaveInProgress = false;

            WaveEnded?.Invoke(this, EventArgs.Empty);

            

            OnWaveEnded(this, EventArgs.Empty);


            if (_WaveNumber >= _TotalWavesInLevel /* && player.IsDead */)
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

            if (_WaveNumber >= _TotalWavesInLevel)
            {
                //HUD.RevealVictory();
            }
        }
    }


    private void CalculateTotalEnemiesInWave()
    {
        _TotalEnemiesInWave = 0;
        foreach (EnemySpawner spawner in _EnemySpawners)
        {
            _TotalEnemiesInWave += spawner.EnemiesInCurrentWave();
        }
    }

    private void CalculateWaveReward()
    {
        if (_EnemySpawners.Count > 0)
        {
            _WaveReward = _EnemySpawners[0].WaveReward(); //TODO: This needs to be changed when we update the wave system.
        }
    }

    private void FindAllSpawners()
    {
        _EnemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None).ToList();
    }


    private void OnWaveEnded(object sender, EventArgs e)
    {
        if (_WaveNumber >= _TotalWavesInLevel)
        {
            //TODO: Add game win state
            return;
        }
 
        

        GameManager.Instance.MoneySystem.AddCurrency(_WaveReward);
 
        NextWave.Instance.EnableButton();
    }



    public int TotalWavesInLevel { get { return _TotalWavesInLevel; } }

    public int WaveNumber { get { return _WaveNumber; } }
    public bool IsWaveInProgress { get { return _WaveInProgress; } }

    public int NumEnemiesKilledInWave { get { return _EnemiesKilled; } }
    public int NumEnemiesReachedGoalInWave { get { return _EnemiesReachedGoal; } }
    public int TotalEnemiesInWave { get { return _TotalEnemiesInWave; } }
   
    public int TotalEnemiesKilledInLevel { get { return _TotalCatsDistracted; } }
    public int TotalEnemiesReachedGoalInLevel { get { return _TotalEnemiesReachedGoal; } }

    public float SecondsElapsedSinceLevelStarted { get { return _SecondsSinceLevelStart; } }
    public float SecondsElapsedSinceWaveStarted { get { return _SecondsSinceWaveStart; } }

}

