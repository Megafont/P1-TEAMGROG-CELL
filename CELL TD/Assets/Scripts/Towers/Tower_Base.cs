using Assets.Scripts.Towers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(StateMachine))]
public class Tower_Base : MonoBehaviour
{
    [Tooltip("This tower's stats information")]
    [SerializeField] protected TowerInfo_Base _TowerInfo;
    
    [SerializeField]
    protected SphereCollider range;

    [SerializeField]
    protected GameObject rangeShower;//TODO find better option later

    [SerializeField]
    private AudioClip _placementClip;

    [SerializeField]
    private AudioSource _audioPlayer;


    protected Type _TargetEnemyType = typeof(Enemy_Base);

    protected Vector3 targetDirection;
    public List<GameObject> targets;

    protected SphereCollider _Collider;

    protected StateMachine _stateMachine;
    protected int _TowerLevel = 1;

    protected float _Health;
    protected float _MaxHealth;
    protected float _DamageValue;
    protected float _FireRate;
    protected float _Range;
    protected int _NumberOfTargets;
    protected float _NextUpgradeCost;
    protected float _RefundPercentage; // The percentage of the build cost that is recovered when you destroy the tower.
    protected float _RefundAmount; // The amount of nutrients recovere when destroying the tower.


    
    public virtual void Start()
    {
        _audioPlayer.clip = _placementClip;
        _audioPlayer.Play();

        Destroy(rangeShower);
        InitTowerStats();
    }

    public virtual void Update()
    {
        //Remove duds
        if (targets.Count > 0)
        {
            if (!targets[0].gameObject)
            {
                targets.Remove(targets[0]);
            }
        }
    }

    private void OnEnable()
    {
        // This corrects the problem with our prefabs. For example, the laser tower
        // has a scale of 500. It's collider has a radius of 6. This effectively means
        // the true size of the collider is radius = 30,000. This adjusts the collider
        // radius by simply dividing it by the gameObject's scale. It doesn't matter
        // whether we use x, y, or z here since it is a sphere.
        _Collider = GetComponent<SphereCollider>();
        _Collider.radius = _Collider.radius / transform.localScale.x;
        if (_stateMachine == null)
        {
            _stateMachine = GetComponent<StateMachine>();
            if (_stateMachine == null)
                throw new Exception($"The tower \"{gameObject.name}\" does not have a state machine component!");

            InitStateMachine();
        }


        EnableTargetDetection();        
    }

    private void OnDisable()
    {
        DisableTargetDetection();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == 6)
        {
            OnNewTargetEnteredRange(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == 6)
        {
            OnTargetWentOutOfRange(collider.gameObject);

            targets.Remove(collider.gameObject);
        }
    }

    /// <summary>
    /// Displays the TowerUI when the user clicks on the tower.
    /// </summary>
    private void OnMouseUp()
    {
        TowerUI.ShowTowerUI(this);
    }

    /// <summary>
    /// Initializes the stats for this tower.
    /// Subclasses should override this function to init stats specific to that tower type.
    /// </summary>
    protected virtual void InitTowerStats()
    {
        _Health = _TowerInfo.BaseMaxHealth;
        _MaxHealth = _TowerInfo.BaseMaxHealth;
        _DamageValue = _TowerInfo.BaseDamageValue;
        _FireRate = _TowerInfo.BaseFireRate;
        _Range = _TowerInfo.BaseRange;
        _NumberOfTargets = _TowerInfo.BaseNumberOfTargets;
        _NextUpgradeCost = _TowerInfo.LevelUpDefinitions[0].UpgradeCost;
        _RefundPercentage = _TowerInfo.BaseRefundPercentage;
        _RefundAmount = _TowerInfo.BuildCost * _RefundPercentage;

        _TowerLevel = 1;
    }

    /// <summary>
    /// This function is overriden by subclasses to allow them to setup the state machine with their own states.
    /// </summary>
    protected virtual void InitStateMachine()
    {
        // Create tower states.
        TowerState_Active_Base activeState = new TowerState_Active_Base(this);
        TowerState_Disabled_Base disabledState = new TowerState_Disabled_Base(this);
        TowerState_Idle_Base idleState = new TowerState_Idle_Base(this);
        TowerState_Upgrading_Base upgradingState = new TowerState_Upgrading_Base(this);


        // Create and register transitions.
        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        _stateMachine.AddTransitionFromState(idleState, new Transition(activeState, () => targets.Count > 0));
        _stateMachine.AddTransitionFromState(disabledState, new Transition(idleState, () => IsTargetDetectionEnabled));

        _stateMachine.AddTransitionFromAnyState(new Transition(disabledState, () => !IsTargetDetectionEnabled));
        _stateMachine.AddTransitionFromAnyState(new Transition(idleState, () => IsTargetDetectionEnabled));

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        // Tell state machine to write in the debug console every time it exits or enters a state.
        _stateMachine.EnableDebugLogging = true;

        // Set the starting state.
        _stateMachine.SetState(idleState);
    }

    /// <summary>
    /// This function exists so that it can be overriden in subclasses. 
    /// </summary>
    /// <remarks>
    /// The purpose of is function is to allow a given tower type to have its own filters on targets.
    /// 
    /// NOTE: Here in the tower base class this function simply adds the target to the list,
    ///       as the base class doesn't need to do any filtering of targets that are in range.
    ///       This defines the default behavior for towers that do not override this function.
    /// </remarks>
    /// <param name="target">The target game object to verify and add to the list.</param>
    protected virtual void OnNewTargetEnteredRange(GameObject target)
    {
        targets.Add(target);
    }

    /// <summary>
    /// This function is just an event hander that subclasses can override to be notified when
    /// a target moves out of range. So it's like OnNewTargetEnteredRange(), but the opposite.
    /// </summary>
    /// <remarks>
    /// 
    /// NOTE: Subclasses DO NOT need to remove the target from targets.
    ///       This class does that right after it calls this event handler.
    ///       For example, laser tower has an ActiveTargets list, so it must remove
    ///       said target from that list in its override of this method.
    /// </remarks>
    /// <param name="target"></param>
    protected virtual void OnTargetWentOutOfRange(GameObject target)
    {
    }

    /// <summary>
    /// This function is an event handler that subclasses can override to be
    /// notified when a target has "died".
    /// </summary>
    /// <param name="target"></param>
    protected virtual void OnTargetHasDied(GameObject target)
    {

    }

    void OnMouseEnter()
    {
        //gameObject.GetComponentInParent<TowerBase>().hoveredOver = true;
        //gameObject.GetComponent<Renderer>().material = gameObject.GetComponentInParent<TowerBase>().towerHovered;
    }

    void OnMouseExit()
    {
        //gameObject.GetComponentInParent<TowerBase>().hoveredOver = false;
        //gameObject.GetComponent<Renderer>().material = gameObject.GetComponentInParent<TowerBase>().towerNotHovered;
    }

    void OnMouseUpAsButton()
    {
        if (enabled)
        {
            

        }
    }

    private void OnEnemyDied(object sender, EventArgs e)
    {
        OnTargetHasDied((GameObject) sender);

        targets.Remove(sender as GameObject);
    }

    public void OnDestroy()
    {
        Destroy(this);
    }

    public virtual void EnableTargetDetection()
    {
        _Collider.enabled = true;
        targets.Clear();
    }

    public virtual void DisableTargetDetection()
    {
        _Collider.enabled = false;
        targets.Clear();
    }

    /// <summary>
    /// Applies a level up to this tower.
    /// </summary>
    /// <remarks>
    /// This function only handles stats that are common to all tower types.
    /// Each tower type has an override of this function that will call this base class version
    /// first, and then once that returns it will then handle all stats specific to that tower type.
    /// </remarks>
    /// <param name="upgradeDef"></param>
    public virtual void Upgrade(TowerUpgradeDefinition upgradeDef)
    {
        // Increment the tower's level.
        _TowerLevel++;

        //Update Cost
        if (_TowerLevel-1 < _TowerInfo.LevelUpDefinitions.Count)
        {
            _NextUpgradeCost = _TowerInfo.LevelUpDefinitions[_TowerLevel - 1].UpgradeCost;
        }

        // Iterate through all of the stat upgrades in this tower upgrade definition.
        foreach (TowerStatUpgradeDefinition statUpgradeDef in upgradeDef.StatUpgradeDefinitions)
        {
            // Check which stat needs to be updated next.
            switch (statUpgradeDef.TowerStat)
            {
                case TowerStats.DamageValue:
                    _DamageValue += statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.FireRate:
                    _FireRate += statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.MaxHealth:
                    _MaxHealth += statUpgradeDef.UpgradeAmount;
                    _Health += statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.NumberOfTargets:
                    _NumberOfTargets += (int) statUpgradeDef.UpgradeAmount;
                    break;
                case TowerStats.RefundPercentage:
                    _RefundPercentage += statUpgradeDef.UpgradeAmount;
                    break;

                default:
                    // If we encountered a stat type that isn't common to all tower types, then simply do nothing.
                    // The subclass' version of this function will handle it after calling this base class method.
                    break;
            }

        }
    }



    public float BuildCost { get { return _TowerInfo.BuildCost; } }
    public float RefundPercentage { get { return _RefundPercentage; } }

    public float RefundAmount {  get { return _RefundAmount; } }
    public float NextUpgradeCost { get { return _NextUpgradeCost; } }
    public float Health 
    { 
        get { return _Health; } 
        set 
        { 
            _Health = value; 

            // Is this tower dead?
            if (_Health <= 0)
            {
                // If this tower is selected in the TowerUI, then close the UI.
                if (TowerUI.Instance.ClickedTower == this)
                {
                    TowerUI.HideTowerUI();
                }

                // Destroy this tower.
                Destroy(gameObject);                
            }
        } 
    }

    public float MaxHealth { get { return _MaxHealth; } }

    public float DamageValue { get { return _DamageValue; } }
    public float FireRate { get { return _FireRate; } }
    public TowerInfo_Base TowerInfo { get { return _TowerInfo; } }

    public int NumberOfTargets { get { return _NumberOfTargets; } }
    public bool IsTargetDetectionEnabled { get { return _Collider.enabled; } }
    public int TowerLevel { get { return _TowerLevel; } }

    public Type TargetEnemyType
    {
        get { return _TargetEnemyType; }
        set 
        {
            if (!typeof(Enemy_Base).IsAssignableFrom(value))
            {
                throw new Exception($"The passed in enemy type is not a subclass of EnemyBase! The tower in question is \"{gameObject.name}\" of type {this.GetType()}");
            }

            _TargetEnemyType = value;
        }
    }

    public TowerTypes TowerType { get { return _TowerInfo.TowerType; } }
}
