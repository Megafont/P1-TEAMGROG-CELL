using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all fungi-type enemies
/// </summary>
public class Fungi_Base : Enemy_Base, IFungi
{
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

        // Do initialization here.
    }

    protected override void KillEnemy(EnemyDeathTypes type)
    {
        // I had to add this if statement, because otherwise the fungi was splitting when reaching the goal and spawning spores.
        // The spores then caused a null reference exception because they couldn't find the next node since there isn't one at that point.
        if (type == EnemyDeathTypes.KilledByPlayer)
        {
            for (int i = 0; i < EnemyInfo_Fungi.SporesPerBurst; i++)
            {
                var newEnemy = Instantiate(_sporeEnemy);
                newEnemy.transform.position = new Vector3(transform.position.x + Random.Range(-1.0f, 1.0f), transform.position.y, transform.position.z + Random.Range(-1.0f, 1.0f));
                newEnemy.GetComponent<Enemy_Base>()._NextWayPoint = _NextWayPoint;
                newEnemy.GetComponent<Enemy_Base>()._spawnedEnemy = true;
                WaveManager.Instance.EnemyAdded();
            }
            base.KillEnemy(type);
        }
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


    public EnemyInfo_Fungi EnemyInfo_Fungi
    {
        get { return EnemyInfo as EnemyInfo_Fungi; }
    }
}
