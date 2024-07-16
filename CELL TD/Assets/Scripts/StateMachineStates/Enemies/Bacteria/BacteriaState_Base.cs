
using UnityEngine;


/// <summary>
/// This is the base class that all Bacteria states inherit from.
/// </summary>
public abstract class BacteriaState_Base : EnemyState_Base
{
    new protected Bacteria_Base _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parent">The state needs a reference to its parent Bacteria object so it can call methods on it.</param>
    public BacteriaState_Base(Bacteria_Base parent)
        : base(parent)
    {
        _parent = parent;
    }


    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }

}

