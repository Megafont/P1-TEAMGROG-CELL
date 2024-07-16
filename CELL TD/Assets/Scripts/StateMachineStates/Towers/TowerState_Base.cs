
using UnityEngine;


/// <summary>
/// This is the base class that all state machine states have at the base of their inheritance heirarchy.
/// </summary>
public abstract class TowerState_Base : State_Base
{
    protected Tower_Base _parentTower;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parentTower">The state needs a reference to its parent tower so it can call methods on it.</param>
    public TowerState_Base(Tower_Base parentTower)
        : base(parentTower.gameObject)
    {
        _parentTower = parentTower;
    }

    public override void OnEnter()
    {
        _parentTower.EnableTargetDetection();
    }
}

