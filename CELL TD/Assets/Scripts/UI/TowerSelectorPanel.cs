using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class manages the Tower Selector panel.
/// </summary>
public class TowerSelectorPanel : MonoBehaviour
{
    [Tooltip("This is the UI element that the buttons will be placed in.")]
    [SerializeField]
    private Transform _ButtonsParent;

    [SerializeField]
    private GameObject _TowerSelectorButtonPrefab;

    [SerializeField]
    private GameObject _placer;


    private List<TowerInfo_Base> _TowerInfosCollection;

    /// <summary>
    /// This holds a reference to the tower selector UI's panel.
    /// </summary>
    private Transform _TowerSelectorPanel;

    /// <summary>
    /// This holds a reference to the tower info popup object.
    /// </summary>
    private TowerInfoPopup _TowerInfoPopupUI;

    /// <summary>
    /// This holds a reference to whichever tower button the mouse
    /// is on. This is used to position the tower info popup properly.
    /// </summary>
    private TowerSelectorButton _HoveredButton;

    /// <summary>
    /// This holds a reference to the tower info for whichever tower type the mouse is on.
    /// If the mouse is not on any of the buttons, this field will be null.
    /// </summary>
    private TowerInfo_Base _PopupTowerInfo;



    private void Awake()
    {
        _TowerSelectorPanel = transform.Find("Panel");

        _TowerInfoPopupUI = GetComponentInChildren<TowerInfoPopup>();

        // Load all of the TowerInfo_Base scriptable objects (including ones that are
        // subclasses of TowerInfo_Base) in the project, even if they
        // aren't inside the TowerInfos folder.
        _TowerInfosCollection = Resources.LoadAll<TowerInfo_Base>("").ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateTowerButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates the tower buttons.
    /// </summary>
    private void CreateTowerButtons()
    {
        for (int i = 0; i < _TowerInfosCollection.Count; i++)
        {
            TowerSelectorButton button = Instantiate(_TowerSelectorButtonPrefab, _ButtonsParent).GetComponent<TowerSelectorButton>();
            button.image.sprite = _TowerInfosCollection[i].DisplayIcon;

            // Store the tower type in the button's TowerType property.
            button.TowerType = _TowerInfosCollection[i].TowerType;
            button.TowerPrefab = _TowerInfosCollection[i].Prefab;
            button.TowerInfo = _TowerInfosCollection[i];

            button.onClick.AddListener(() =>
            {
                OnTowerSelectButtonClicked(button);
            });
            
            button.OnMouseEnter += (sender, eventData) =>
            {
                OnMouseOverTowerButton(button);
            };
            button.OnMouseExit += (sender, eventData) =>
            {
                OnMouseExitedTowerButton(button);
            };
        }
    }

    private void OnTowerSelectButtonClicked(TowerSelectorButton button)
    {
        if (button.TowerPrefab && button.TowerInfo)
        {
            GameObject newPlacer = GameObject.FindGameObjectWithTag("Placer");
            if (newPlacer)
            {
                if (newPlacer.GetComponent<Placer>().info == button.TowerInfo)
                {
                    Destroy(newPlacer);
                    return;
                }
                Destroy(newPlacer);
            }
            newPlacer = Instantiate(_placer);
            newPlacer.GetComponent<Placer>().tower = button.TowerPrefab;
            newPlacer.GetComponent<Placer>().info = button.TowerInfo;
        }
        else
        {
            Debug.LogError($"The tower type \"{Enum.GetName(typeof(TowerTypes), button.TowerType)}\" is not implemented yet in TowerSelector.OnTowerSelectButtonClicked!");
        }
    }

    private void OnMouseOverTowerButton(TowerSelectorButton button)
    {
        _HoveredButton = button;

        _TowerInfoPopupUI.ShowPopup(button);
    }

    private void OnMouseExitedTowerButton(TowerSelectorButton button)
    {
        _PopupTowerInfo = null;

        _TowerInfoPopupUI.ClosePopup();
    }



    public Rect PanelSize { get { return _TowerSelectorPanel.GetComponent<RectTransform>().rect; } }

}
