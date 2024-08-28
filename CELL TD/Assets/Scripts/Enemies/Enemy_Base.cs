using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

// This fixes the ambiguity between System.Random and UnityEngine.Random by
// telling it to use the Unity one.
using Random = UnityEngine.Random;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(StatusEffectsManager))]
public class Enemy_Base : MonoBehaviour, IEnemy
{
    public enum EnemyDeathTypes
    {
        KilledByPlayer,
        ReachedGoal,
    }


    public static bool ShowDistractednessBar = true;

    // This event is static so we don't need to subscribe to every enemy instance's OnEnemyDied event.
    public static event EventHandler OnEnemyDied;
    public static event EventHandler OnEnemyReachedGoal;



    [Tooltip("This enemy's stats information")]
    [SerializeField] private EnemyInfo_Base _EnemyInfo;
   

    protected NavMeshAgent _NavMeshAgent;

    protected float _AttackDamage;
    protected float _AttackSpeed;
    protected float _AttackRange;
    protected float _MovementSpeed;
    protected float _RewardAmount;
    protected float _WayPointArrivedDistance;

    [SerializeField] protected float _Health; // This enemy's current health.
    protected bool _IsDead = false; // If this enemy has been defeated or not.

    protected float _DistanceFromNextWayPoint = 0f;
    public WayPoint _NextWayPoint; //Made this public so that spawned spores would stop skipping waypoints
    public bool _spawnedEnemy = false; //If spawned, uses parent nextWayPoint instead

    protected bool _isATarget = false;
    [SerializeField] protected SpawnedUnit _TargetUnit;

    private AudioSource enemyAudio;

    public List<GameObject> slowingEntities;
    public List<GameObject> stoppingEntities;

    private StateMachine _StateMachine;

    private HealthSystem _PlayerHealthSystem;

    [SerializeField]
    private AudioSource _audioPlayer;
    [SerializeField]
    private AudioClip _fatal;
    [SerializeField]
    private AudioClip _damage;

    private Animator animator;

    protected void Awake()
    {
        InitEnemyStats();

        animator = GetComponentInChildren<Animator>();

        IsDead = false;

        _PlayerHealthSystem = FindAnyObjectByType<HealthSystem>();

        _NavMeshAgent = GetComponent<NavMeshAgent>();

        enemyAudio = GetComponent<AudioSource>();


        _StateMachine = GetComponent<StateMachine>();
        if (_StateMachine == null)
            throw new Exception($"The enemy \"{gameObject.name}\" does not have a StateMachine component!");

        InitStateMachine();
    }

    // Start is called before the first frame update
    protected void Start()    
    {
        // Find the closest WayPoint and start moving there.
        if (!_spawnedEnemy)
        {
            FindNearestWayPoint();

            // This prevents an issue where if the enemy spawns to close to the nearest waypoint, then he won't move since he is already at his next node.
            // This catches that and tells him to go to the next node.
            if (HasReachedDestination())
            {
                GetNextWaypoint();
            }
        }

        // Now rotate the enemy to face the next waypoint.
        if (_NextWayPoint != null)
        {
            transform.LookAt(_NextWayPoint.transform.position);
        }


        _NavMeshAgent.SetDestination(_NextWayPoint.transform.position);
        _NavMeshAgent.speed = _EnemyInfo.MovementSpeed;
    }

    /// <summary>
    /// Initializes the stats for this enemy.
    /// Subclasses should override this function to init stats specific to that enemy type.
    /// </summary>
    protected virtual void InitEnemyStats()
    {
        _AttackDamage = _EnemyInfo.AttackDamage;
        _AttackSpeed = _EnemyInfo.AttackSpeed;
        _AttackRange = _EnemyInfo.AttackRange;
        _MovementSpeed = _EnemyInfo.MovementSpeed;
        _Health = _EnemyInfo.MaxHealth;
        _RewardAmount = _EnemyInfo.RewardAmount;
        _WayPointArrivedDistance = _EnemyInfo.WayPointArrivedDistance;
    }

    /// <summary>
    /// This function is overriden by subclasses to allow them to setup the state machine with their own states.
    /// </summary>
    protected virtual void InitStateMachine()
    {
        // Create enemy states.
        EnemyState_Idle_Base idleState = new EnemyState_Idle_Base(this);
        EnemyState_Dead_Base deadState = new EnemyState_Dead_Base(this);
        EnemyState_Moving movingState = new EnemyState_Moving(this);
        EnemyState_Slowed slowedState = new EnemyState_Slowed(this);
        EnemyState_Stopped stoppedState = new EnemyState_Stopped(this);


        // Create and register transitions.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        _StateMachine.AddTransitionFromState(movingState, new Transition(slowedState, () => slowingEntities.Count > 0 && stoppingEntities.Count == 0));
        _StateMachine.AddTransitionFromState(movingState, new Transition(stoppedState, () => stoppingEntities.Count > 0));
        _StateMachine.AddTransitionFromState(slowedState, new Transition(stoppedState, () => stoppingEntities.Count > 0));
        _StateMachine.AddTransitionFromState(stoppedState, new Transition(slowedState, () => stoppingEntities.Count == 0 &&
                                                                                             slowingEntities.Count > 0));

        _StateMachine.AddTransitionFromAnyState(new Transition(movingState, () => slowingEntities.Count == 0 && stoppingEntities.Count == 0));

        // If health is at or below 0, then switch to the dead state regardless of what state this enemy is currently in.        
        _StateMachine.AddTransitionFromAnyState(new Transition(deadState, () => _Health <= 0));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        _StateMachine.EnableDebugLogging = true;

        // This is necessary since we only have one state and no transitions for now.
        // Mouse over the AllowUnknownStates property for more info.
        _StateMachine.AllowUnknownStates = true;


        // Set the starting state.
        _StateMachine.SetState(idleState);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_Health <= 0 && !_IsDead)
        {
            IsDead = true;
            return;
        }


        if (_NextWayPoint != null && stoppingEntities.Count <= 0)
        {
            // I'm subtracting 1 at the end here to account for the fact that the enemy is just over 1 unit away from the waypoint when directly on top of it.
            _DistanceFromNextWayPoint = Vector3.Distance(transform.position, _NextWayPoint.transform.position) - 1f;

            if (HasReachedDestination())
            {
                GetNextWaypoint();

                // Check for null in case we are already at the last WayPoint, as GetNextWayPoint()
                // returns null if there is no next WayPoint.
                if (_NextWayPoint != null)
                    _NavMeshAgent.SetDestination(_NextWayPoint.transform.position);
            }
        }
        else
        {
            _DistanceFromNextWayPoint = 0f;
        }


        if (stoppingEntities.Count > 0 || slowingEntities.Count > 0)
        {
            CleanEntitiesLists();
        }
    }

    private void CleanEntitiesLists()
    {
        // Remove any stopping entities that are now null (meaning they have been destroyed).
        for (int i = stoppingEntities.Count - 1; i >= 0; i--)
        {
            if (stoppingEntities[i] == null)
            {
                stoppingEntities.RemoveAt(i);
            }

        } // end for i

        // Remove any slowing entities that are now null (meaning they have been destroyed).
        for (int j = slowingEntities.Count - 1; j >= 0; j--)
        {
            if (slowingEntities[j] == null)
            {
                slowingEntities.RemoveAt(j);
            }

        } // end for i

    }

    //I am intending this function to be called from either the tower or the projectile that the tower fires
    public void ApplyDamage(float damageValue, Tower_Base targetingTower)
    {
        _Health -= damageValue;
        _audioPlayer.clip = _damage;
        _audioPlayer.pitch = Health / _EnemyInfo.MaxHealth + .5f*(_EnemyInfo.MaxHealth/(Health+_EnemyInfo.MaxHealth));
        _audioPlayer.Play();
        if (_Health <= 0 && !_IsDead)
        {
            StartCoroutine(PlayDeathSound());

            if (targetingTower)
            {
                targetingTower.targets.Remove(this.gameObject);
            }

            KillEnemy(EnemyDeathTypes.KilledByPlayer);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        // Did this enemy reach the goal?
        if (other.gameObject.CompareTag("Goal"))
        {
            _PlayerHealthSystem.TakeDamage((int) _AttackDamage);
            KillEnemy(EnemyDeathTypes.ReachedGoal);
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            SimpleProjectile projRef = other.gameObject.GetComponent<SimpleProjectile>();
            ApplyDamage(projRef._damage,projRef._owner);

            //Subtract piercing
            projRef._piercing -= 1;
        }
    }

    protected virtual void KillEnemy(EnemyDeathTypes type)
    {
        if (IsDead)
            return;


        // Prevents this function from running twice in rare cases, causing this enemy's death to count as more than one.
        IsDead = true;

        if(type == EnemyDeathTypes.KilledByPlayer)
        {        
            // Give the player money if this enemy's GivesMoneyOnDeath flag is true.
            if (GivesMoneyOnDeath)
                GameManager.Instance.MoneySystem.AddCurrency(_EnemyInfo.CurrencyGain);

            // Fire the OnEnemyDied event.
            OnEnemyDied?.Invoke(this, EventArgs.Empty);
        }
        else if(type == EnemyDeathTypes.ReachedGoal)
        {
            OnEnemyReachedGoal?.Invoke(this, EventArgs.Empty);
        }

        if(animator != null)
        {
            animator.SetTrigger("isDead");
            // Destroy this enemy after one second
            StartCoroutine(DestroyAfterDelay(1f));
        } else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        

    }

    // delay the death of the object for the death animation
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
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
            _NextWayPoint = _NextWayPoint.NextWayPoints[Random.Range(0, count)];
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
        return _DistanceFromNextWayPoint <= _WayPointArrivedDistance;// &&
               //_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }

    public void SetAsTarget(SpawnedUnit unit)
    {
        _isATarget = true;
        _TargetUnit = unit;
        StartCoroutine(Attack());
    }

    public void SetNotTarget(SpawnedUnit unit)
    {
        stoppingEntities.Remove(unit.gameObject);
        _isATarget = false;
        _TargetUnit = null;
    }

    public void ResetMovementSpeed()
    {
        MovementSpeed = _EnemyInfo.MovementSpeed;
    }

    IEnumerator Attack()
    {
        if (Vector3.Distance(transform.position, _TargetUnit.transform.position) <= _AttackRange)
        {
            _TargetUnit.ApplyDamage(AttackDamage);
        }

        yield return new WaitForSeconds(_AttackSpeed);
        if (_TargetUnit != null)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator PlayDeathSound()
    {
        _NavMeshAgent.speed = 0;

        yield return new WaitForSeconds(0.5f);

        KillEnemy(EnemyDeathTypes.KilledByPlayer);
    }
   


    public float AttackDamage { get { return _EnemyInfo.AttackDamage; } }
    public float MovementSpeed 
    { 
        get { return _MovementSpeed; } 
        set 
        { 
            _MovementSpeed = value; 
            _NavMeshAgent.speed = _MovementSpeed;
        } 
    }
    public EnemyTypes EnemyType { get { return _EnemyInfo.Type; } }    
    public float Health { get { return _Health; } }
    public bool IsBacteria { get; protected set; } = false;
    public bool IsDead { get; protected set; } = false;
    public bool IsFungi { get; protected set; } = false;
    public bool IsVirus { get; protected set; } = false;
    public float MaxHealth { get { return _EnemyInfo.MaxHealth; } }
    public float RewardAmount { get { return _EnemyInfo.RewardAmount; } }

    public bool IsATarget {  get { return _isATarget;  } }

    public WayPoint NextWayPoint 
    { 
        get { return _NextWayPoint; } 
        set
        { 
            _NextWayPoint = value; 
        }
    }

    public EnemyInfo_Base EnemyInfo { get { return _EnemyInfo; } }

    /// <summary>
    /// This property is set to false by code that spawns in new enemies that aren't from an EnemySpawner. Examples would be a virus converting a macrophage into a new virus,
    /// or a boss enemy that spawns reinforcements via the BossSpawner_Base script. That code should set this to true, so that the code that
    /// gives the player money will know that this enemy should not give the player money.
    /// </summary>
    public bool GivesMoneyOnDeath { get; set; } = true;
}