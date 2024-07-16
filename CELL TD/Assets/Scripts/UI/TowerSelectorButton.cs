using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// This is a custom button class that simply adds the ability for the button to know which tower type
/// it is associated with.
/// </summary>
/// <remarks>
/// </remarks>
public class TowerSelectorButton : Button, IPointerEnterHandler, IPointerExitHandler
{
    public event EventHandler OnMouseEnter;
    public event EventHandler OnMouseExit;


    // The tower type associated with this button
    public TowerTypes TowerType;
    public GameObject TowerPrefab;
    public TowerInfo_Base TowerInfo;


    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter?.Invoke(this, EventArgs.Empty);

        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameObject newPlacer = GameObject.FindGameObjectWithTag("Placer");
        if (!newPlacer) //Duct tape solution
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        OnMouseExit?.Invoke(this, EventArgs.Empty);
        base.OnPointerExit(eventData);
    }
}
