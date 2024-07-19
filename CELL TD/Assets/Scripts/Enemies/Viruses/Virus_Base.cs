using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// This is the base class for all virus-type enemies
/// </summary>
public class Virus_Base : Enemy_Base, IVirus
{
    List<GameObject> _PlayerUnitsInRange = new List<GameObject>();

    
    private float _ConversionTimer = 0.0f;


    protected new void Awake()
    {
        base.Awake();

        // Do initialization here.
        IsVirus = true;
    }

    protected new void Start()
    {
        base.Start();

        // Do initialization here.        
    }

    protected new void Update()
    {
        base.Update();

        _ConversionTimer += Time.deltaTime;
        if (_ConversionTimer > 1.0f)

        {
            _ConversionTimer = 0.0f;
            TryToConvertPlayerUnitsInRange();
        }
    }

    /// <summary>
    /// Initializes stats specific to virus-type enemies.
    /// Stats common to all enemy types should be initialized in the base class version of this method.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitEnemyStats()
    {
        base.InitEnemyStats();

        // Init virus-specific enemy stats here.
    }

    /// <summary>
    /// Initializes the state machine of this enemy.
    /// This function is called by the base class.
    /// </summary>
    protected override void InitStateMachine()
    {
        // This probably isn't needed.
        //base.InitStateMachine();
    }


    protected new void OnTriggerEnter(Collider other)
    {
        if (!_PlayerUnitsInRange.Contains(other.gameObject) &&
            other.gameObject.layer == LayerMask.NameToLayer("Player Units"))
        {
            _PlayerUnitsInRange.Add(other.gameObject);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        _PlayerUnitsInRange.Remove(other.gameObject);
    }

    private void TryToConvertPlayerUnitsInRange()
    {
        List<RaycastHit> playerUnits = Physics.SphereCastAll(transform.position, EnemyInfo_Virus.ConversionRadius, Vector3.up, 0.1f, LayerMask.GetMask("Player Units")).ToList();
        if (playerUnits == null || playerUnits.Count < 1)
            return;


        // This loop iterates through all player units that are known to be within range.
        // It does so in reverse order, so that when we remove one from from the list after converting
        // it into a virus, we move on to the index before it on the next loop iteration.
        // This way, the indices of the objects we still have to check will not get shifted
        // up one slot in the array like they would if we iterate from the start of the list to the end.
        for (int i = playerUnits.Count - 1; i >= 0; i--)
        {
            GameObject playerUnit = playerUnits[i].collider.gameObject;

            if (playerUnit == null)
            {
                playerUnits.RemoveAt(i);
            }
            else if (Vector3.Distance(transform.position, playerUnit.transform.position) <= EnemyInfo_Virus.ConversionRadius)
            {
                // Draw a random number to see if the virus will convert the player unit into another virus.
                float f = Random.Range(0f, 1f);


                if (f <= EnemyInfo_Virus.ChanceOfConversion)
                {
                    //Debug.Log($"<color=orange>Converting player unit \"{playerUnit.name}\" into a virus!</color>", playerUnit);

                    // Spawn a new virus at the location of the player unit.
                    GameObject newVirus = Instantiate(EnemyInfo_Virus.Prefab, playerUnit.transform.position, Quaternion.identity, transform.parent);
                    newVirus.GetComponent<Virus_Base>().NextWayPoint = NextWayPoint;

                    // Remove the player unit object from the list.
                    playerUnits.RemoveAt(i);

                    // Destroy the player unit by dealing damage to it. This is because if we simply called GameObject.Destroy() on it, then
                    // its OnUnitDied event never gets called. The tower ends up not knowing it was destroyed, so its unit counter gets off.
                    // After this happens a few times, it will never spawn any more units because it thinks it still has these ones.
                    playerUnit.GetComponent<SpawnedUnit>().ApplyDamage(10000f); // This number is stupidly big just to make sure it dies.                   
                }
                else
                {
                    // Remove the player unit from the list since it already had its chance at being converted.
                    playerUnits.RemoveAt(i);
                }
            }

        } // end for i
    }
     
    

    public EnemyInfo_Virus EnemyInfo_Virus
    {
        get {  return EnemyInfo as EnemyInfo_Virus; }       
    }
}
