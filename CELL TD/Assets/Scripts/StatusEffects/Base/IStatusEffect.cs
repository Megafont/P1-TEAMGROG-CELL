using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// A simple interface for easily accessing any status effect instance.
/// </summary>
public interface IStatusEffect
{
    /// <summary>
    /// This method is called when the status effect is first applied, making it a great place
    /// to initialize it, for example setting up visual effects created by this status effect.
    /// </summary>
    abstract void OnEffectStart();

    /// <summary>
    /// This method is called when the status effect expires, making it a great place
    /// to do any clean up, for example removing visual effects created by this status effect.
    /// </summary>
    abstract void OnEffectEnd();

    /// <summary>
    /// The StatusEffectsManager script on an enemy object will call this function to apply the status effect.
    /// It gets called once per frame as long as the status effect is active.
    /// </summary>
    /// <exception cref=">ArgumentNullException">if targetEnemy is null</exception>
    abstract void Update();

    /// <summary>
    /// This method stacks another status effect instance on top of this one.
    /// It gets called by the StatusEffectManager on the enemy object when necessary if it has stacking enabled.
    /// </summary>
    /// <param name="statusEffectInstance"></param>
    /// <exception cref=">ArgumentException">if statusEffectInstance is snot the same type as this status effect instance</exception>
    /// <exception cref=">ArgumentNullException">if statusEffectInstance is null</exception>
    abstract void Stack(IStatusEffect statusEffectInstance);

    /// <summary>
    /// Adds the specified amount of time (in seconds) to the duration timer for this status effect.
    /// </summary>
    /// <param name="amount"></param>
    void AddToTimer(float amount);


    /// <summary>
    /// Returns the amount of time (in seconds) that has elapsed since this status effect was applied to the enemy.
    /// </summary>
    float ElapsedTime { get; }

    /// <summary>
    /// Gets the amount of time remaining (in seconds) until this status effect expires.
    /// </summary>    
    float TimeRemaining { get; }
    /// <summary>
    /// Indicates whether or not this status effect has expired.
    /// </summary>
    bool IsExpired { get; }

    /// <summary>
    /// Indicates whether or not this status effect has been stacked.
    /// </summary>
    bool IsStacked { get; }

    /// <summary>
    /// Returns the number of times this status effect has been stacked.
    /// </summary>
    public int StackCount { get; }

    /// <summary>
    /// Gets the StatusEffectInfo of this status effect.
    /// </summary>
    StatusEffectInfo_Base StatusEffectInfo { get; }


}
