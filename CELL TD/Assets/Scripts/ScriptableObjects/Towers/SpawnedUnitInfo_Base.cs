using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any tower-spawned unit of any type.
/// </summary>
[CreateAssetMenu(fileName = "New CelInfo_Base", menuName = "Spawned Unit Info Assets/New SpawnedUnitInfo_Base Asset")]
public class SpawnedUnitInfo_Base : ScriptableObject
{
    [Header("General Unit Info")]

    public string DisplayName; // The name displayed for this Spawned Unit in the UI
    public SpawnedUnitTypes Type; // The type of this Spawned Unit.
    public Sprite UiIcon; // The icon used for this Spawned Unit in the UI
    public GameObject Prefab; // The prefab for this SpawnedUnit type

    [Header("Unit Stats")]

    [Min(0)]
    public float AttackDamage = 2f;
    [Min(0)]
    public float AttackSpeed = 1f;
    [Min(0)]
    public float BaseMovementSpeed = 1f;
    [Min(0)]
    public float MaxHealth = 50f;
    [Min(0)]
    public float WayPointArrivedDistance = 2f; // This is how close this unit must get to the next waypoint for it to be considered as having arrived there

    [Tooltip("The status effect applied by this unit.")]
    [SerializeField]
    public StatusEffectInfo_Stopped StatusEffect;
}