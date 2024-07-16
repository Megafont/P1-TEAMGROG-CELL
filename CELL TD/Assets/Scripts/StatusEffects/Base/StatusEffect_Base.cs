using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This is the base class for all status effect instances.
/// </summary>
public abstract class StatusEffect_Base : IStatusEffect
{
    /// <summary>
    /// The target of this status effect.
    /// </summary>
    protected object _Target;

    /// <summary>
    /// Contains the definition of this status effect.
    /// </summary>
    protected StatusEffectInfo_Base _StatusEffectInfo;

    /// <summary>
    /// Number of times this status effect has been stacked.
    /// </summary>
    protected int _StackCount;

    /// <summary>
    /// How long this status effect will last.
    /// </summary>
    protected float _Duration;

    /// <summary>
    /// The timer for this status effect.
    /// </summary>
    protected float _Timer = 0f;


    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="statusEffectInfo">The definition containing the details about this status effect.</param>
    /// <param name="target">The target object of this status effect.</param>
    public StatusEffect_Base(StatusEffectInfo_Base statusEffectInfo, object target)
    {
        if (statusEffectInfo == null)
            throw new ArgumentNullException(nameof(statusEffectInfo));
        if (target == null)
            throw new ArgumentNullException(nameof(target));


        _Target = target;
        _StatusEffectInfo = statusEffectInfo;
        _Duration = statusEffectInfo.Duration;
    }



    /// <summary>
    /// This method is called when the status effect is first applied, making it a great place
    /// to initialize it, for example setting up visual effects created by this status effect.
    /// </summary>
    public abstract void OnEffectStart();

    /// <summary>
    /// This method is called when the status effect expires, making it a great place
    /// to do any clean up, for example removing visual effects created by this status effect.
    /// </summary>
    public abstract void OnEffectEnd();

    /// <summary>
    /// The StatusEffectsManager script on an enemy object will call this function to update the status effect once every frame.
    /// This is basically the same as the Update() method of a MonoBehaviour.
    /// </summary>
    /// <remarks>
    /// For example, a status effect that does damage over time might override this method to implement a simple
    /// timer that damages the target once every second.
    /// </remarks>
    /// <param name="targetEnemy">The enemy the effect is being applied to.</param>
    public abstract void Update();

    /// <summary>
    /// This method stacks another status effect instance on top of this one.    
    /// </summary>
    /// <param name="statusEffectInstance"></param>
    /// <exception cref=">ArgumentException">if statusEffectInstance is snot the same type as this status effect instance</exception>
    /// <exception cref=">ArgumentNullException">if statusEffectInstance is null</exception>
    public abstract void Stack(IStatusEffect statusEffectInstance);


    /// <summary>
    /// Checks whether the specified StatusEffectInstance is the same type as this one.
    /// </summary>
    /// <param name="statusEffectInstance"></param>
    /// <returns></returns>
    public bool IsSameType(IStatusEffect statusEffectInstance)
    {
        if (statusEffectInstance == null)
            throw new ArgumentNullException(nameof(statusEffectInstance));


        return _StatusEffectInfo.Type == statusEffectInstance.StatusEffectInfo.Type;
    }

    /// <summary>
    /// Adds the specified amount of time (in seconds) to the duration timer for this status effect.
    /// </summary>
    /// <param name="amount"></param>
    public void AddToTimer(float amount)
    {
        //Debug.Log($"Timer: ({_Timer} + {amount}) = {_Timer + amount} / {_Duration}");
        _Timer += amount;
    }



    /// <summary>
    /// Returns the amount of time (in seconds) that has elapsed since this status effect was applied to the enemy.
    /// </summary>
    public float ElapsedTime
    {
        get
        {
            return _Timer;
        }
    }

    /// <summary>
    /// Gets the amount of time remaining (in seconds) until this status effect expires.
    /// </summary>
    public float TimeRemaining
    { 
        get 
        { 
            return _Duration - _Timer; 
        } 
    }

    /// <summary>
    /// Indicates whether or not this status effect has expired.
    /// </summary>
    public bool IsExpired
    {
        get
        {
            return _Timer >= _Duration;
        }
    }

    /// <summary>
    /// Indicates whether or not this status effect is stacked.
    /// </summary>
    public bool IsStacked
    {
        get
        {
            return _StackCount > 0;
        }
    }

    /// <summary>
    /// Returns the number of times this status effect has been stacked.
    /// </summary>
    public int StackCount
    {
        get
        {
            return _StackCount;
        }
    }

    /// <summary>
    /// Gets the StatusEffectInfo of this status effect.
    /// </summary>
    public StatusEffectInfo_Base StatusEffectInfo
    {
        get
        {
            return _StatusEffectInfo;
        }
    }

}
