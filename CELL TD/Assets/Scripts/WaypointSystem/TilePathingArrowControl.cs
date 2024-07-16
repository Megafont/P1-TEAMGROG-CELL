using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathingArrowControl : MonoBehaviour
{
    private bool up;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private bool waiting = false;


    // Start is called before the first frame update
    void Start()
    {
        upPosition = new Vector3(0, 40, -0.25f);
        downPosition = new Vector3(0, 0, -0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(lineController());
        
    }

    IEnumerator lineController()
    {
        if(waiting)
        {
            yield return new WaitForEndOfFrame();
        } else
        {
            LineRenderer line = this.gameObject.GetComponentInChildren<LineRenderer>();
            if (up)
            {
                line.transform.localPosition = downPosition;
                up = false;
            }
            else
            {
                line.transform.localPosition = upPosition;
                up = true;
            }
            waiting = true;
            yield return new WaitForSeconds(2);
            waiting = false;
        }
        
    }
}
