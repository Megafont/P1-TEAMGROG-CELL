
using UnityEngine;


/// <summary>
/// This is the base class that all state machine states have at the base of their inheritance heirarchy.
/// </summary>
public abstract class State_Base : IState
{
    // A reference to the object this state belongs to. This allows the state to manipulate the object by calling methods on it or setting properties.
    protected GameObject _parent;



    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="parentTower">The state needs a reference to its parent object so it can call methods on it.</param>
    public State_Base(GameObject parent)
    {
        _parent = parent;
    }


    // These overridable methods are automatically called by the state machine.
    public virtual void OnEnter() 
    {
       // Debug.Log($"Entered state \"{this.Name}\"."); 
    }
    public virtual void OnUpdate() { }
    public virtual void OnExit() 
    { 
       // Debug.Log($"Exited state \"{this.Name}\"."); 
    }



    /// <summary>
    /// This property returns the C# class name of this state. This is useful when you need to check the current state of the state machine.
    /// </summary>
    public string Name { get { return this.GetType().Name; } }
}

