using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// A simple class that manages the status effects on a given enemy.
/// </summary>
public class StatusEffectsManager : MonoBehaviour
{
    /// <summary>
    /// This flag indicates whether or not the game allows status effects to stack on top of each other if
    /// one is added to an enemy that already has an active instance of that status effect on it.
    /// </summary>
    public static bool AllowStacking = true;



    /// <summary>
    /// Holds the list of status effects currently on the enemy.
    /// </summary>
    public List<IStatusEffect> _StatusEffects = new List<IStatusEffect>();



    private void Update()
    {
        // If this StatusEffectsManager is not enabled, then simply return.
        if (!IsEnabled)
            return;


        // Update all status effects on this enemy.
        UpdateAllStatusEffects();
    }

    /// <summary>
    /// Increments the timers on all status effects and removes any that have expired.
    /// </summary>
    private void UpdateAllStatusEffects()
    {
        float deltaTime = Time.deltaTime;

        // Increment the timers on all status effects and remove any that have expired.
        for (int i = _StatusEffects.Count - 1; i >= 0; i--)
        {
            IStatusEffect statusEffect = _StatusEffects[i];

            // Add the delta time of the last frame to this status effect's timer.
            statusEffect.AddToTimer(deltaTime);

            // If this status effect is now expired, then remove it from the list.
            if (statusEffect.IsExpired)
            {
                statusEffect.OnEffectEnd();

                _StatusEffects.RemoveAt(i);
            }
            else
            {
                // This status effect has not updated, so call its Update() method.
                statusEffect.Update();
            }

        } // end for i
    }

    /// <summary>
    /// Adds a new status effect to this enemy.
    /// </summary>
    /// <param name="statusEffectInstance">The status effect instance to add.</param>
    public void ApplyStatusEffect(IStatusEffect statusEffectInstance)
    {
        if (!IsEnabled)
            return;


        // Check if this enemy already has this type of status effect on it.
        for (int i = 0; i < _StatusEffects.Count; i++)
        {
            IStatusEffect statusEffect = _StatusEffects[i];

            // Does this target already have an active status effect of this type?
            if (statusEffect.StatusEffectInfo.Type == statusEffectInstance.StatusEffectInfo.Type)
            {
                // The new status effect is the same type as the one at this index. So check if stacking is enabled?
                if (AllowStacking)
                {
                    // If it does, add the new status effect to it.
                    statusEffect.Stack(statusEffectInstance);

                    //Debug.Log($"Stacked status effect \"{statusEffectInstance.StatusEffectInfo.DisplayName}\".  Active status effects: {StatusEffectsCount}");
                }

                // Simply return now since there is no need to check the rest of the list.
                return;
            }

        } // end for i


        // This enemy does not already have an instance of this type of status effect on it, so just add it.
        _StatusEffects.Add((StatusEffect_Base) statusEffectInstance);

        statusEffectInstance.OnEffectStart();

        //Debug.Log($"Added status effect \"{statusEffectInstance.StatusEffectInfo.DisplayName}\".  Active status effects: {StatusEffectsCount}");
    }

    /// <summary>
    /// Clears all status effects from this enemy.
    /// </summary>
    public void ClearAllStatusEffects()
    {
        _StatusEffects.Clear();
    }

    /// <summary>
    /// Removes a specified type of status effect from this enemy if it is present.
    /// </summary>
    /// <param name="statusEffectType"></param>
    public void RemoveStatusEffectOfType(StatusEffectTypes statusEffectType)
    {
        _StatusEffects.RemoveAll(x => x.StatusEffectInfo.Type == statusEffectType);
    }



    /// <summary>
    /// Whether or not this status effects manager is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Returns the number of status effects that are currently active on this enemy.
    /// </summary>
    public int StatusEffectsCount
    {
        get
        {
            return _StatusEffects.Count;
        }
    }

}
