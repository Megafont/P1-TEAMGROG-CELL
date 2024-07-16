using System.Collections;
using System;
using UnityEngine;
using Assets.Scripts.Towers;

public class Macrophage_UnitSpawnerTower : Tower_Base
{
    [Header("Spawned Unit Information")]
    [SerializeField, Tooltip("The prefab of the units to spawn")]
    private GameObject unitPrefab;

    [SerializeField, Tooltip("Maximum number of units to spawn")]
    private int maxUnits;
    private int currentUnits;

    [SerializeField, Tooltip("Unit spawn point")]
    private GameObject spawnPoint;

    [Tooltip("This spawned unit's stats information")]
    [SerializeField] public SpawnedUnitInfo_Base _SpawnedUnitInfo;

    private float _SpawnedUnitHealth;
    private float _SpawnedUnitDamage;

    public override void Start()
    {
        _SpawnedUnitHealth = unitPrefab.GetComponent<SpawnedUnit>().unitInfo.MaxHealth;
        _SpawnedUnitDamage = unitPrefab.GetComponent<SpawnedUnit>().unitInfo.AttackDamage;
        currentUnits = 0;
        StartCoroutine(Spawner());
        base.Start();
    }


    IEnumerator Spawner()
    {
    
        if (currentUnits >= maxUnits)
        {
            StopCoroutine(Spawner());
        }
        else if (currentUnits < maxUnits)
        {
            GameObject newUnit = Instantiate(unitPrefab, spawnPoint.transform.position, Quaternion.identity, gameObject.transform);
            newUnit.GetComponent<SpawnedUnit>().parent = gameObject;
            newUnit.GetComponent<SpawnedUnit>().UnitDied += OnUnitDied;
            newUnit.GetComponent<SpawnedUnit>().unitInfo.AttackDamage = _SpawnedUnitDamage;
            newUnit.GetComponent<SpawnedUnit>().unitInfo.MaxHealth = _SpawnedUnitHealth;
            currentUnits++;
        }
        yield return new WaitForSeconds(FireRate);
        StartCoroutine(Spawner());

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
                case TowerStats.UnitAmount:
                    maxUnits += (int)statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.UnitDamage:
                    _SpawnedUnitDamage += statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.UnitHealth:
                    _SpawnedUnitHealth += statUpgradeDef.UpgradeAmount;
                    break;
                default:
                    // If we encountered a stat type that isn't specific to this tower type, then simply do nothing.
                    break;

            } // end switch

        } // end foreach TowerStatUpgradeDefinition
    }

    private void OnUnitDied(object sender, EventArgs e)
    {
        currentUnits--;
    }


}