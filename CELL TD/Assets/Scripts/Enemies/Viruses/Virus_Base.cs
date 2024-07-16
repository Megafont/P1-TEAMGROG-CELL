using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all virus-type enemies
/// </summary>
public class Virus_Base : Enemy_Base, IVirus
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsVirus = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.        
    }

    /// <summary>
    /// Initializes stats specific to virus-type enemies.
    /// Stats common to all enemy types should be initialized in the base class version of this method.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();

        // Init virus-specific enemy stats here.
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
