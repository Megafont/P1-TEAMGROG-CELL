using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class WayPoint : MonoBehaviour
{
    public List<WayPoint> NextWayPoints = new List<WayPoint>();



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        float arrowHeadLength = 0.5f;
        float arrowHeadWidth = 0.5f;
        float verticalOffset = 0.75f;   // Sets how much to offset the gizmmos vertically. This is used to make them appear a little bit above the ground in the editor.


        Vector3 wayPointGizmoPos = transform.position + Vector3.up * verticalOffset;

        Gizmos.color = new Color32(0, 100, 255, 128);
        Gizmos.DrawSphere(wayPointGizmoPos, 0.5f);


        Gizmos.color = new Color32(255, 150, 0, 200);
        foreach (WayPoint nextPoint in NextWayPoints)
        {
            Vector3 nextWayPointGizmoPos = nextPoint.transform.position + Vector3.up * verticalOffset;
            Vector3 direction = nextWayPointGizmoPos - wayPointGizmoPos;
            Vector3 directionNormalized = direction.normalized;


            Gizmos.DrawLine(wayPointGizmoPos, nextWayPointGizmoPos);

            // Get the halfway point between this waypoint and the next.
            Vector3 midPoint = wayPointGizmoPos + (direction / 2);

            // Calculate the right/left vectors from the perspective of direction.
            Vector3 right = new Vector3(directionNormalized.z, 0, -directionNormalized.x);
            Vector3 left = -right;

            // Calculate the center point of the back of the arrow head.
            Vector3 backCenterPoint = midPoint - (directionNormalized * arrowHeadLength);

            // Calculate arrow head corners.
            Vector3 rightCorner = backCenterPoint + (right * (arrowHeadWidth / 2));
            Vector3 leftCorner = backCenterPoint + (left * (arrowHeadWidth / 2));

            // Draw an arrow head here.
            Gizmos.color = new Color32(255, 255, 0, 128);
            Gizmos.DrawLine(midPoint, rightCorner);
            Gizmos.DrawLine(midPoint, leftCorner);
            Gizmos.DrawLine(rightCorner, leftCorner);
        }
    }
}
