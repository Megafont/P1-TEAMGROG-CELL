using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class StatusEffect_Stopped : StatusEffect_Base
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="statusEffectInfo">The definition containing the details about this status effect.</param>
    /// <param name="target">The target object of this status effect.</param>
    public StatusEffect_Stopped(StatusEffectInfo_Stopped statusEffectInfo, object target)
        : base(statusEffectInfo, target)
    {
        if (statusEffectInfo == null)
            throw new ArgumentNullException(nameof(statusEffectInfo));

        // Make sure the passed in StatusEffectInfo is of the correct type.
        if (statusEffectInfo.Type != StatusEffectTypes.Stopped)
            throw new ArgumentException(nameof(statusEffectInfo.Type));
    }

    /// <summary>
    /// This method is called when the status effect is first applied, making it a great place
    /// to initialize it, for example setting up visual effects created by this status effect.
    /// </summary>
    public override void OnEffectStart()
    {

    }

    /// <summary>
    /// This method is called when the status effect expires, making it a great place
    /// to do any clean up, for example removing visual effects created by this status effect.
    /// </summary>
    public override void OnEffectEnd()
    {
        TargetEnemy.ResetMovementSpeed();
    }

    /// <summary>
    /// The StatusEffectsManager script on an enemy object will call this function to apply the status effect.
    /// It gets called once per frame as long as the status effect is active.
    /// </summary>
    /// <param name="targetEnemy">The enemy the effect is being applied to.</param>
    /// <exception cref=">ArgumentNullException">if targetEnemy is null</exception>
    public override void Update()
    {
        // The code below is pseudocode for what should happen here

        if (TargetEnemy.MovementSpeed > StatusEffectInfo.MaxMoveSpeed)
        {
            TargetEnemy.MovementSpeed = StatusEffectInfo.MaxMoveSpeed;
        }
    }

    /// <summary>
    /// This method stacks another status effect instance on top of this one.
    /// It gets called by the StatusEffectManager on the enemy object when necessary if it has stacking enabled.
    /// </summary>
    /// <param name="statusEffectInstance"></param>
    /// <exception cref=">ArgumentException">if statusEffectInstance is snot the same type as this status effect instance</exception>
    /// <exception cref=">ArgumentNullException">if statusEffectInstance is null</exception>
    public override void Stack(IStatusEffect statusEffectInstance)
    {
        // First, make sure the specified status effect instance is the same type as this one. Otherwise it makes no sense to stack them.
        if (!IsSameType(statusEffectInstance))
            throw new ArgumentException(nameof(statusEffectInstance));


        _Duration += statusEffectInstance.StatusEffectInfo.Duration;
    }



    /// <summary>
    /// Gets the StatusEffectInfo of this status effect.
    /// This property uses the new keyword to intentionally hide the base class version of this property.
    /// </summary>
    new public StatusEffectInfo_Stopped StatusEffectInfo
    {
        get
        {
            return _StatusEffectInfo as StatusEffectInfo_Stopped;
        }
    }


    public Enemy_Base TargetEnemy { get { return (Enemy_Base)_Target; } }
}
