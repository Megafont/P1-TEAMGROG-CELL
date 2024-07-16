using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This class is a simple component that yI used to setup simple tests
/// to verify that WayPointUtils is working properly.
/// </summary>
/// <remarks>
/// Simply set the parameters of this component in the inspector, and then enter play mode.
/// This class will immediately run the test and print the results in the console.
/// </remarks>
public class WayPointUtilsTester : MonoBehaviour
{
    [SerializeField]
    private WayPoint _WayPointA;
    [SerializeField]
    private WayPoint _WayPointB;



    // Start is called before the first frame update
    void Start()
    {
        if (_WayPointA != null && _WayPointB != null)
        {
            WaypointUtils.WayPointCompareResults result = WaypointUtils.CompareWayPointPositions(_WayPointA, _WayPointB);
            Debug.Log($"WayPointTest Result = {result}");
        }
        else
        {
            Debug.LogError("Either WayPointA, WayPointB, or both are not set in the inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RunTest()
    {

    }

 
 
}
