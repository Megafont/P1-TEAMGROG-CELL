using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


/// <summary>
/// This simple helper class closes the TowerUI when the user clicks outside of the panel.
/// </summary>
public class TowerUI_Helper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _MouseIsOverPanel;



    private void Update()
    {
        CheckForClickOutsideWindow();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _MouseIsOverPanel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _MouseIsOverPanel = false;
    }

    private void CheckForClickOutsideWindow()
    {
        //Debug.Log("@^$%^@$%&^@$^%@$&");

        // If the user clicks outside of this panel, then close it.
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            TowerUI.Instance.IsOpen &&
            !_MouseIsOverPanel)
        {
            TowerUI.HideTowerUI();
        }
    }

}
