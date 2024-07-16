using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partchooser : MonoBehaviour
{
    [SerializeField]
    private GameObject partHolder;

    private void Start()
    {
        //Get All parts in partholder
        List<GameObject> parts = new List<GameObject>();
        foreach (Transform child in partHolder.transform)
        {
            parts.Add(child.gameObject);
        }

        //Pick random
        GameObject chosenItem = parts[Random.Range(0, parts.Count - 1)];

        //Remove others
        foreach (GameObject part in parts)
        {
            if (part != chosenItem)
            {
                Destroy(part);
            }
        }
    }
}
