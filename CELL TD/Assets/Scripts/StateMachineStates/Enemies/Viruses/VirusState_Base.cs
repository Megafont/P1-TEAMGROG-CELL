
using UnityEngine;


/// <summary>
/// This is the base class that all Virus states inherit from.
/// </summary>
public abstract class VirusState_Base : EnemyState_Base
{
    new protected Virus_Base _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parent">The state needs a reference to its parent Virus object so it can call methods on it.</param>
    public VirusState_Base(Virus_Base parent)
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

