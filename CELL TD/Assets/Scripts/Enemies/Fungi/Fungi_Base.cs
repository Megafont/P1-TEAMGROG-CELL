using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all fungi-type enemies
/// </summary>
public class Fungi_Base : Enemy_Base, IFungi
{
    private EnemyInfo_Fungi _infoRef;
    [SerializeField]
    private GameObject _sporeEnemy;
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsFungi = true;
    }

    new void Start()
    {
        base.Start();
        _infoRef = (EnemyInfo_Fungi)_EnemyInfo;

        // Do initialization here.
    }

    protected override void KillEnemy(int type)
    {
        for (int i = 0; i < _infoRef.SporesPerBurst; i++)
        {
            var newEnemy = Instantiate(_sporeEnemy);
            newEnemy.transform.position = new Vector3(transform.position.x+Random.Range(-1.0f,1.0f), transform.position.y, transform.position.z + Random.Range(-1.0f, 1.0f));
            newEnemy.GetComponent<Enemy_Base>()._NextWayPoint = _NextWayPoint;
            newEnemy.GetComponent<Enemy_Base>()._spawnedEnemy = true;
            WaveManager.Instance.EnemyAdded();
        }
        base.KillEnemy(type);
    }

    /// <summary>
    /// Initializes stats specific to fungi-type enemies.
    /// Stats common to all enemy types should be initialized in the base class version of this method.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();

        // Init fungi-specific enemy stats here.
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
