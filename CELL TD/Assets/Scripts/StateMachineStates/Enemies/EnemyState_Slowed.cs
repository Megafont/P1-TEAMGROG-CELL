using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// This is the Slowed State providing functionality for enemies that are slowed
/// </summary>
public class EnemyState_Slowed : EnemyState_Base
{
    public EnemyState_Slowed(Enemy_Base parent)
        : base(parent)
    {

    }

    public override void OnEnter()
    {
        UpdateSlowedModifier();
    }

    public override void OnExit()
    {
        _parent.gameObject.GetComponent<NavMeshAgent>().speed = _parent.GetComponent<Enemy_Base>().MovementSpeed;
    }

    public override void OnUpdate()
    {
        UpdateSlowedModifier();
    }

    private void UpdateSlowedModifier()
    {
        Enemy_Base enemy = _parent.GetComponent<Enemy_Base>();
        float modifier = 1;
        foreach (GameObject obj in enemy.slowingEntities)
        {
            //Logic to apply slowed effect
            if (obj != null)
            {
                
            }
        }
        _parent.GetComponent<NavMeshAgent>().speed = enemy.MovementSpeed / modifier;
    }

}