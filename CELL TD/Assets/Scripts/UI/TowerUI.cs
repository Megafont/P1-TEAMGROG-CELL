using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using Assets.Scripts.Towers;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance { get; set; }


    [Header("UI References - Header")]

    [Tooltip("The panel of this dialog.")]
    [SerializeField]
    private GameObject _Panel;

    [Tooltip("The image element to display the tower icon in")]
    [SerializeField]
    private Image _IconImage;

    [Tooltip("The text element to display the title text in")]
    [SerializeField]
    private TextMeshProUGUI _TitleText;

    [Tooltip("The text element to display the tower's level in")]
    [SerializeField]
    private TextMeshProUGUI _LevelText;

    [Tooltip("The text element to display the tower's health in")]
    [SerializeField]
    private TextMeshProUGUI _HealthText;

    [Tooltip("The text element to display the tower's damage value in")]
    [SerializeField]
    private TextMeshProUGUI _DamageText;

    [Tooltip("The text element to display the tower's fire rate in")]
    [SerializeField]
    private TextMeshProUGUI _FireRateText;

    [Tooltip("The text element to display the number of simultaneous targets the tower can handle in")]
    [SerializeField]
    private TextMeshProUGUI _NumberOfTargetsText;


    [Header("UI References - Upgrade Section")]

    [Tooltip("The top level parent object of the Upgrade section of the UI")]
    [SerializeField]
    private Transform _UpgradeSectionParent;

    [Tooltip("The UI separator line element that separates the upgrade section from the destruction section.")]
    [SerializeField]
    private Transform _UpgradeSectionSeparator;

    [Tooltip("The parent element of all the text elements in the UI's Upgrade section.")]
    [SerializeField]
    private Transform _UpgradeStatsParent;

    [Tooltip("The text element to display the next upgrade's cost in.")]
    [SerializeField]
    private TextMeshProUGUI _UpgradeCostText;


    [Header("UI References - Destruction Section")]

    [Tooltip("The text element to display the destruction refund amount in.")]
    [SerializeField]
    private TextMeshProUGUI _RefundText;


    [Header("Prefabs")]

    [Tooltip("The UI prefab to use for displaying an upgrade stat.")]
    [SerializeField]
    private GameObject _UpgradeStatPrefab;


    private float _NextUpgradeCost;

    // This list is a simple object pool to re-use the stat row Ui prefabs used to show the stats effected by the next upgade
    private List<Transform> _UpgradeStatUiObjects = new List<Transform>();



    /// <summary>
    /// Holds a reference to the tower that was clicked on to open this dialog
    /// </summary>
    public Tower_Base ClickedTower { get; private set; }

    /// <summary>
    /// Returns true if the tower UI is currently open
    /// </summary>
    public bool IsOpen
    {
        get
        {
            return gameObject.activeSelf;
        }
    }



    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogWarning("There is already a TowerUI instance in this scene. Self destructing...");
            Destroy(gameObject);
            return;
        }


        Instance = this;

        // Hide this dialog when the game starts
        gameObject.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitUI()
    {
        // Fill in the UI's header section
        _IconImage.sprite = ClickedTower.TowerInfo.DisplayIcon;
        _TitleText.text = ClickedTower.TowerInfo.DisplayName;
        _LevelText.text = ClickedTower.TowerLevel.ToString();
        _HealthText.text = $"{ClickedTower.Health.ToString()} / {ClickedTower.MaxHealth.ToString()}";
        _DamageText.text = ClickedTower.DamageValue.ToString();
        _FireRateText.text = ClickedTower.FireRate.ToString();
        _NumberOfTargetsText.text = ClickedTower.NumberOfTargets.ToString();


        // Fill in the UI's upgrade section
        _UpgradeCostText.text = ((int) ClickedTower.NextUpgradeCost).ToString();
        UpdateUpgradeSection();

        // Fill in the UI's destruction section
        _RefundText.text = ((int) ClickedTower.RefundAmount).ToString();
    }


    /// <summary>
    /// This offset is simply used to take into account the UI elements that are always in this
    /// section (the cost display, and a vertical spacer just below it).
    /// </summary>
    private static int _UiIndexOffset = 2;

    /// <summary>
    /// Updates the UI in the upgrade section by filling it out with details about the
    /// next upgrade for the selected tower.
    /// </summary>
    public void UpdateUpgradeSection()
    {
        ResetUpgradeSectionStatsUI();

        
        int towerLevel = ClickedTower.TowerLevel;
        bool towerStillHasUpgradesAvailable = towerLevel - 1 < ClickedTower.TowerInfo.LevelUpDefinitions.Count;

        // If there are no level ups left for this tower, then simply hide that section of the UI and return.
        _UpgradeSectionParent.gameObject.SetActive(towerStillHasUpgradesAvailable);
        _UpgradeSectionSeparator.gameObject.SetActive(towerStillHasUpgradesAvailable); // Also hide the separator line between the upgrade and destruction sections
        if (!towerStillHasUpgradesAvailable)
        {
            return;
        }

        TowerUpgradeDefinition nextUpgradeDef = ClickedTower.TowerInfo.LevelUpDefinitions[towerLevel - 1];

        // Get the number of stats effected by thsi levelup.
        int statCount = nextUpgradeDef.StatUpgradeDefinitions.Count;

        // Iterate through all the stats affected by this levelup, and setup a UI element for each
        int uiIndex = _UiIndexOffset;
        foreach (TowerStatUpgradeDefinition statUpgradeDef in nextUpgradeDef.StatUpgradeDefinitions)
        {
            Transform uiObj = null;

            // Get the UI object
            if (uiIndex < _UpgradeStatsParent.childCount)
            {
                uiObj = _UpgradeStatsParent.GetChild(uiIndex);
            }
            // We don't have any more stat display UI objects
            else
            {
                // Check if we still have any currently unused instances in our pool
                if (_UpgradeStatUiObjects.Count > 0)
                {
                    uiObj = _UpgradeStatUiObjects[0];
                    _UpgradeStatUiObjects.RemoveAt(0);
                }
                else
                {
                    // Create a new one
                    uiObj = Instantiate(_UpgradeStatPrefab, _UpgradeStatsParent).transform;
                }
            }


            // Give the uiObj a name so it shows up nice in the inspector when testing.
            uiObj.gameObject.name = "Stat Row - " + statUpgradeDef.TowerStat.GetStatName();

            // Fill in the uiObj with the next stat's info
            uiObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = statUpgradeDef.TowerStat.GetStatName();
            uiObj.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"+{statUpgradeDef.UpgradeAmount.ToString()}";

            // Parent the uiObj to our parent object for them
            uiObj.SetParent(_UpgradeStatsParent);

            // Make sure the uiObj is activated
            uiObj.gameObject.SetActive(true);


            // Move to the next UI element index
            uiIndex++;

        } // end foreach

    }

    /// <summary>
    /// Resets the UI in the upgrade section by removing the UI elements for all stat upgrades
    /// that are currently there. It skips over the first UI elements, as these are the ones
    /// that are always there (the cost display, and a vertical spacer just below it).
    /// </summary>
    private void ResetUpgradeSectionStatsUI()
    {
        if (_UpgradeStatsParent.childCount <= 1)
            return;


        // NOTE: We start with an offset here to take into account the UI elements that are always
        //       in this section (the cost display, and a vertical spacer just below it).
        //       We don't want to remove those child objects, so this offset skips them.
        for (int i = _UiIndexOffset; i < _UpgradeStatsParent.childCount; i++)
        {
            Transform uiObj = _UpgradeStatsParent.GetChild(i);

            // Deactivate the uiObj
            uiObj.gameObject.SetActive(false);

            // Store the uiObj in our pool to reuse later
            _UpgradeStatUiObjects.Add(uiObj);
        }
    }

    public void OnUpgradeClicked()
    {
        if (GameManager.Instance.MoneySystem.MoneyAmount > ClickedTower.NextUpgradeCost)
        {
            GameManager.Instance.MoneySystem.SubtractCurrency((int)ClickedTower.NextUpgradeCost);

            ClickedTower.Upgrade(ClickedTower.TowerInfo.LevelUpDefinitions[ClickedTower.TowerLevel - 1]);

            HideTowerUI();
        }
        else
        {
            GameManager.Instance.MoneySystem.SubtractCurrency(999999999);//This will show the player that they do not have enough money
        }
    }

    public void OnDestroyClicked()
    {
        GameManager.Instance.MoneySystem.AddCurrency((int) ClickedTower.RefundAmount);

        Destroy(ClickedTower.gameObject);

        HideTowerUI();
    }

    public void OnCloseClicked()
    {
        HideTowerUI();
    }


    public static void ShowTowerUI(Tower_Base tower)
    {
        Instance.ClickedTower = tower;
        Instance.InitUI();

        // Display the UI
        Instance.gameObject.SetActive(true);
    }

    public static void HideTowerUI()
    {
        Instance.ClickedTower = null;
        Instance.gameObject.SetActive(false);
    }

}
