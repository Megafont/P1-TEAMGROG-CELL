
using UnityEngine;


/// <summary>
/// This is the base class that all Fungi states inherit from.
/// </summary>
public abstract class FungiState_Base : EnemyState_Base
{
    new protected Fungi_Base _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parent">The state needs a reference to its parent Fungi object so it can call methods on it.</param>
    public FungiState_Base(Fungi_Base parent)
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

