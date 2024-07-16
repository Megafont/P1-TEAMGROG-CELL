using Assets.Scripts.Towers;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Lymphocyte_SlowingAOETower : Tower_Base
{
    float _EnemySearchTimer;
    private int effectStrength = 0;

    public override void Update()
    {
        base.Update();


        _EnemySearchTimer += Time.deltaTime;

        // Just use the fire rate stat as the frequency at which this tower attacks enemies.
        if (_EnemySearchTimer >= _FireRate)
        {
            // Find all enemies within range and apply the status effect to them.
            FindEnemiesInRange();

            // Reset the timer.
            _EnemySearchTimer = 0;
        }
    }

    /// <summary>
    /// Find all enemies within range of the tower.
    /// </summary>
    private void FindEnemiesInRange()
    {       
        // Find all enemies withint range. We use a layer mask for the Enemies layer since we only care about enemies.
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 
                                                  _Range, 
                                                  Vector3.up, 
                                                  0, 
                                                  LayerMask.GetMask("Enemies"), 
                                                  QueryTriggerInteraction.Collide);
        for (int i = 0; i < hits.Length; i++)
        {
            //Debug.Log("Hit: " + hits[i].collider.gameObject.name);

            Enemy_Base enemy = hits[i].collider.gameObject.GetComponent<Enemy_Base>();
            if (enemy != null)
            {
                StatusEffectsManager effectsMgr = enemy.GetComponent<StatusEffectsManager>();
                if (effectsMgr != null)
                {
                    switch (TowerLevel)
                    {
                        case 1:
                            effectsMgr.ApplyStatusEffect(new StatusEffect_SlowedMoveSpeed(TowerInfo.StatusEffect, enemy));
                            break;
                        case 2:
                            effectsMgr.ApplyStatusEffect(new StatusEffect_SlowedMoveSpeed(TowerInfo.StatusEffect1, enemy));
                            break;
                        case 3:
                            effectsMgr.ApplyStatusEffect(new StatusEffect_SlowedMoveSpeed(TowerInfo.StatusEffect2, enemy));
                            break;
                        case 4:
                            effectsMgr.ApplyStatusEffect(new StatusEffect_SlowedMoveSpeed(TowerInfo.StatusEffect3, enemy));
                            break;
                        default:
                            effectsMgr.ApplyStatusEffect(new StatusEffect_SlowedMoveSpeed(TowerInfo.StatusEffect, enemy));
                            break;
                    }
                }
            }

        } // end for i
    }

    /// <summary>
    /// Applies a level up to this tower.
    /// </summary>
    /// <remarks>
    /// This function only handles stats specific to this tower type.
    /// It calls the base class version of this method to handle stats common to all tower types.
    /// </remarks>
    /// <param name="upgradeDef"></param>
    public override void Upgrade(TowerUpgradeDefinition upgradeDef)
    {
        // First call the base class version of this method to handle stats common to all tower types.
        base.Upgrade(upgradeDef);


        // Iterate through all of the stat upgrades in this tower upgrade definition.
        foreach (TowerStatUpgradeDefinition statUpgradeDef in upgradeDef.StatUpgradeDefinitions)
        {
            // Check which stat needs to be updated next.
            switch (statUpgradeDef.TowerStat)
            {
                case TowerStats.EffectStrength:
                    effectStrength += (int)statUpgradeDef.UpgradeAmount;
                    break;

                default:
                    // If we encountered a stat type that isn't specific to this tower type, then simply do nothing.
                    break;

            } // end switch

        } // end foreach TowerStatUpgradeDefinition
    }



    /// <summary>
    /// This property uses the new keyword to intentionally hide the base class version of this property.
    /// </summary>
    new public TowerInfo_Lymphocyte_SlowingAOETower TowerInfo
    {
        get
        {
            return (TowerInfo_Lymphocyte_SlowingAOETower) _TowerInfo;
        }
    }
}