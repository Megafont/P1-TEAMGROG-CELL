using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;

public class EnemyCounter : MonoBehaviour
{   
    public TextMeshProUGUI enemyCountText;
    public int enemies = 0;
    void Update()
    {
        EnemyDies(this, EventArgs.Empty);
        EnemyReachesGoal(this, EventArgs.Empty);
    }

    void EnemyDies(object Sender, EventArgs a)
    {
        UpdateEnemies();
    }

    void EnemyReachesGoal(object Sender, EventArgs a)
    {
        UpdateEnemies();
    }

    void UpdateEnemies()
    {
        if (WaveManager.Instance)
        {
            enemies = WaveManager.Instance.TotalEnemiesInWave - WaveManager.Instance.NumEnemiesKilledInWave - WaveManager.Instance.NumEnemiesReachedGoalInWave;
            enemyCountText.text = "" + enemies;
        }
    }
}
