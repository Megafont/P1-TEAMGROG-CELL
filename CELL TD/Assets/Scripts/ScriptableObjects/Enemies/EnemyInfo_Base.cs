using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This is a scriptable object that can be used to define basic information about any enemy of any type.
/// However, you should use the BacteriaInfo, FungiInfo, or VirusInfo subclasses instead, depending on the type of your enemy.
/// </summary>
[CreateAssetMenu(fileName = "New EnemyInfo_Base", menuName = "Enemy Info Assets/New EnemyInfo_Base Asset")]
public class EnemyInfo_Base : ScriptableObject
{
    [Header("General Enemy Info")]
    
    public string DisplayName; // The name displayed for this enemy in the UI
    public string Description; // The description displayed in the UI
    public EnemyTypes Type; // The type of this enemy.
    public Sprite UiIcon; // The icon used for this enemy in the UI
    public GameObject Prefab; // The prefab for this enemy type

    [Header("Enemy Base Stats")]

    [Min(0)]
    public float AttackDamage = 2f;
    [Min(0)]
    public float AttackSpeed = 1f;
    [Min(0)]
    public float MovementSpeed = 1f;
    [Min(0)]
    public float MaxHealth = 50f;
    [Min(0)]
    public float RewardAmount = 20f; // How much nutrients the player gets when this enemy is destroyed
    [Min(0)]
    public float WayPointArrivedDistance = 2f; // This is how close this enemy must get to the next waypoint for it to be considered as having arrived there
    [Min(0)]
    public int CurrencyGain = 15; // This is how close this enemy must get to the next waypoint for it to be considered as having arrived there
}
