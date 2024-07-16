using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is an interface that can be used for all enemy objects
/// </summary>
public interface IEnemy
{
    public float AttackDamage { get; }

    public float MovementSpeed { get; }

    public EnemyTypes EnemyType { get; }

    /// <summary>
    /// The current health of the enemy. Call ApplyDamage()/HealDamage() to change it. These methods trigger certain events.
    /// </summary>
    public float Health { get; }

    public bool IsBacteria { get; }
    public bool IsDead { get; }
    public bool IsFungi { get; }
    public bool IsVirus { get; }
    public float MaxHealth { get; }

    public WayPoint NextWayPoint { get; set; }

    public float RewardAmount { get; }
}
