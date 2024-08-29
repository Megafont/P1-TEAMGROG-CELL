using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neutrophil_ProjectileTower : Tower_Base
{

    /// <summary>
    /// Applies a level up to this tower.
    /// </summary>
    /// <remarks>
    /// This function only handles stats specific to this tower type.
    /// It calls the base class version of this method to handle stats common to all tower types.
    /// </remarks>
    /// <param name="upgradeDef"></param>
    /// 
    [SerializeField]
    private GameObject projectile;
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


                default:
                    // If we encountered a stat type that isn't specific to this tower type, then simply do nothing.
                    break;

            } // end switch

        } // end foreach TowerStatUpgradeDefinition
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine("Shoot");
    }

    protected override void AttackTarget(GameObject target)
    {
        // Checks if the target is still within range
        if (target != null)
        {
            // Here you can add logic for the turret to rotate towards the target.
            Vector3 direction = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


    private IEnumerator Shoot()
    {
        while(true)
        {
            if (targets.Count > 0 && targets[0])
            {
                _newModelAnimator.SetBool("isShooting", true);
                // Wait a short time so the animation can start playing so it looks nicely synched up with the launch of the projectile.
                yield return new WaitForSeconds(0.25f);

                // We need to check this again here in case things changed since the previous if statement ran a quarter second ago.
                if (targets.Count > 0 && targets[0])
                {
                    GameObject newProjectile = Instantiate(projectile, transform);
                    SimpleProjectile newInfo = newProjectile.GetComponent<SimpleProjectile>();

                    newInfo._damage = DamageValue;
                    newInfo._direction = Quaternion.LookRotation(targets[0].transform.position - transform.position, Vector3.up);
                    newInfo._size = 1.0f;
                    newInfo._speed = 35.0f;
                    newInfo._piercing = 1;
                    newInfo._owner = this;

                    //targets.Remove(targets[0]);

                    yield return new WaitForSeconds(FireRate);
                }
                _newModelAnimator.SetBool("isShooting", false);
            }
            else
            {
                _newModelAnimator.SetBool("isShooting", false);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}