
using UnityEngine;


/// <summary>
/// This is the base class that all game states inherit from.
/// </summary>
public abstract class GameState_Base : State_Base
{
    new protected GameManager _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parent">The state needs a reference to its parent GameManager object so it can call methods on it.</param>
    public GameState_Base(GameManager parent)
        : base(parent.gameObject)
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

