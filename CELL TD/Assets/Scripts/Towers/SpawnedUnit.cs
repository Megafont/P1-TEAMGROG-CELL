using System;
using System.Collections;

using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(NavMeshAgent))]
public class SpawnedUnit : MonoBehaviour
{

    protected NavMeshAgent _NavMeshAgent;

    [SerializeField] protected float _AttackDamage = 10f;
    [SerializeField] protected float _AttackSpeed = 1f;
    [SerializeField] protected float _BaseMovementSpeed = 3f;
    [SerializeField] protected float _WayPointArrivedDistance = 2f;

    [SerializeField] protected float _Health = 50f; // This unit's current health.

    public SpawnedUnitInfo_Base unitInfo;

    protected float _DistanceFromNextWayPoint = 0f;
    protected WayPoint _NextWayPoint;

    public Macrophage_UnitSpawnerTower tower;
    public GameObject parent;
    private bool isAttacking = false;
    [SerializeField] private Enemy_Base target;

    public event EventHandler UnitDied;

    protected virtual void OnUnitDied(EventArgs e)
    {
        UnitDied?.Invoke(this, e);
    }
    void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
        _NavMeshAgent.speed = _BaseMovementSpeed;
        tower = parent.GetComponent<Macrophage_UnitSpawnerTower>();
    }

    /// <summary>
    /// Initializes the stats for this enemy.
    /// Subclasses should override this function to init stats specific to that enemy type.
    /// Note: Will be utilized later, had some bugs to get it functioning correctly that are related to prefab instantiation
    /// </summary>
    protected virtual void InitUnitStats()
    {
        _AttackDamage = tower._SpawnedUnitInfo.AttackDamage;
        _AttackSpeed = tower._SpawnedUnitInfo.AttackSpeed;
        _BaseMovementSpeed = tower._SpawnedUnitInfo.BaseMovementSpeed;
        _Health = tower._SpawnedUnitInfo.MaxHealth;
        _WayPointArrivedDistance = tower._SpawnedUnitInfo.WayPointArrivedDistance;
    }

    void Update()
    {

        //If the unit has a target that is not in the list, the unit does not have a target
        if (target != null && !tower.targets.Contains(target.gameObject))
        {
            RemoveTarget();

        }
        //If there are enemies in range and the unit does not have a target, find a target
        if (tower.targets.Count > 0 && target == null)
        {
            FindClosestAvailableEnemy();
        }
        if (target != null && tower.targets.Contains(target.gameObject))
        {
            _NavMeshAgent.SetDestination(target.transform.position);
        }
    }

    //Gets the enemy closest to the player that is not stopped and sets it as the target
    private void FindClosestAvailableEnemy()
    {
        GameObject closestEnemy = null;
        float smallestDist = 2000000;
        foreach (GameObject enemy in tower.targets)
        {
            if (enemy != null) //If the cat exists
            {
                if (EnemyDistance(enemy) < smallestDist &&                 //If it is closer to the person than the previous smallest distance
                    !(enemy.GetComponent<Enemy_Base>().IsATarget) &&       //If the enemy is not a target of another person
                    tower.targets.Contains(enemy))                         //If the enemy is in range of the tower  
                {
                    smallestDist = EnemyDistance(enemy);
                    closestEnemy = enemy;
                }
            }

        }
        if (closestEnemy != null) { 
            target = closestEnemy.GetComponent<Enemy_Base>(); //Sets the target to the closest enemy
            //StatusEffectsManager effectsMgr = target.GetComponent<StatusEffectsManager>();
            //if (effectsMgr != null)
            //{
            //    effectsMgr.ApplyStatusEffect(new StatusEffect_Stopped(tower._SpawnedUnitInfo.StatusEffect));
            //}
            //target.stoppingEntities.Add(gameObject);
            _NavMeshAgent.SetDestination(target.transform.position);
            target.GetComponent<Enemy_Base>().SetAsTarget(this);
            StartCoroutine(Attack());
        }
    }
    //Finds the distance between the person and the cat
    private float EnemyDistance(GameObject enemy)
    {
        float distance = 0f;
        float xDist = Mathf.Pow(transform.position.x - enemy.transform.position.x, 2);
        float yDist = Mathf.Pow(transform.position.x - enemy.transform.position.x, 2);
        distance = Mathf.Sqrt(xDist + yDist);
        return distance;
    }
    protected void GetNextWaypoint()
    {
        int count = _NextWayPoint.NextWayPoints.Count;

        if (count == 0)
        {
            _NextWayPoint = null;
        }
        else if (count == 1)
        {
            _NextWayPoint = _NextWayPoint.NextWayPoints[0];
        }
        else // count is greater than 1
        {
            // The current waypoint has multiple next waypoints, so we will
            // select one at random.
            _NextWayPoint = _NextWayPoint.NextWayPoints[UnityEngine.Random.Range(0, count)];
        }

    }

    protected void FindNearestWayPoint()
    {
        float minDistance = float.MaxValue;
        WayPoint nearestWayPoint = null;

        foreach (WayPoint wayPoint in FindObjectsByType<WayPoint>(FindObjectsSortMode.None))
        {
            float distance = Vector3.Distance(transform.position, wayPoint.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWayPoint = wayPoint;
            }
        }


        _NextWayPoint = nearestWayPoint;
    }

    public bool HasReachedDestination()
    {
        return _DistanceFromNextWayPoint <= _WayPointArrivedDistance &&
               _NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    public void RemoveTarget()
    {
        StopAllCoroutines();
        if (target != null)
        {
            target.SetNotTarget(this);
            target.stoppingEntities.Remove(gameObject);
            target = null;

        }
        isAttacking = false;
    }

    public void ApplyDamage(float damage)
    {
        _Health -= damage;
        if (_Health < 0f)
        {
            RemoveTarget();
            // Fire the OnUnitDied event.
            UnitDied?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        if(target == null)
        {
            RemoveTarget();
        } else
        {
            target.ApplyDamage(_AttackDamage, tower);
            StatusEffectsManager effectsMgr = target.GetComponent<StatusEffectsManager>();
            if (effectsMgr != null)
            {
                //effectsMgr.ApplyStatusEffect(new StatusEffect_Stopped(unitInfo.StatusEffect, target));
            }
            _Health -= target.AttackDamage;
            yield return new WaitForSeconds(_AttackSpeed);
            StartCoroutine(Attack());
        }
        
    }


}