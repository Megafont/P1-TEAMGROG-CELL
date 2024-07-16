using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class TowerInfoPopup : MonoBehaviour
{
    [Header("UI References")]

    [SerializeField]
    private TextMeshProUGUI _NameText;
    [SerializeField]
    private TextMeshProUGUI _CostText;
    [SerializeField]
    private TextMeshProUGUI _TargetingTypeText;
    [SerializeField]
    private TextMeshProUGUI _AttackDamageText;

    [Space(10)]

    // The following two fields are used for attack speed or spawn rate depending on the type of the tower.
    [SerializeField]
    private TextMeshProUGUI _AttackSpeedOrSpawnRateLabel;
    [SerializeField]
    private TextMeshProUGUI _AttackSpeedOrSpawnRateText;

    [Space(10)]

    [SerializeField]
    private TextMeshProUGUI _DescriptionText;


    private RectTransform _PopupRectTransform;
    private TowerSelectorPanel _TowerSelectorPanel;



    private void Awake()
    {
        _PopupRectTransform = transform.Find("PopupPanel").GetComponent<RectTransform>();

        _TowerSelectorPanel = FindObjectOfType<TowerSelectorPanel>();       
    }

    // Start is called before the first frame update
    void Start()
    {
        ClosePopup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopup(TowerSelectorButton hoveredButton)
    {
        RectTransform buttonRectTransform = hoveredButton.GetComponent<RectTransform>();

       
        // Position the tower info popup
        _PopupRectTransform.anchoredPosition = new Vector2(-2, 
                                                           buttonRectTransform.anchoredPosition.y);

        // Fill in the tower info on the popup's UI
        TowerInfo_Base towerInfo = hoveredButton.TowerInfo;

        gameObject.SetActive(true);

        _NameText.text = towerInfo.DisplayName;
        _CostText.text = towerInfo.BuildCost.ToString();
        _TargetingTypeText.text = InsertSpacesBeforeAllCapitalLetters(InsertSpacesBeforeAllCapitalLetters(Enum.GetName(typeof(TargetingTypes), towerInfo.TargetingType)));
        _AttackDamageText.text = towerInfo.BaseDamageValue.ToString();


        _AttackSpeedOrSpawnRateLabel.text = towerInfo.TowerType == TowerTypes.Macrophage_UnitSpawnerTower ? "Spawn Rate:"
                                                                                               : "Attack Rate:";
        _AttackSpeedOrSpawnRateText.text = towerInfo.BaseFireRate.ToString();


        _DescriptionText.text = towerInfo.Description;
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// This function is used to add a space before all capital letters in the passed in string (except the first one).
    /// This is useful for turning the name of an enum value into a better display string for the UI.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string InsertSpacesBeforeAllCapitalLetters(string text)
    {
        string result = "";

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            // If this is a capital letter, and it is not the first letter in the string, then insert a space before it.
            if (i > 0 && char.IsUpper(c))
            {
                result += " " + c;
            }
            else
            {
                result += c;
            }

        } // end for i


        return result;
    }
}
