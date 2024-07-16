using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is the base class for all bacteria-type enemies
/// </summary>
public class Bacteria_Base : Enemy_Base, IBacteria    
{
    new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsBacteria = true;
    }

    new void Start()
    {
        base.Start();

        // Do initialization here.
    }

    /// <summary>
    /// Initializes stats specific to bacteria-type enemies.
    /// Stats common to all enemy types should be initialized in the base class version of this method.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();

        // Init bacteria-specific enemy stats here.
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
