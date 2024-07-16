using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


 
public class SettingsWindow : MonoBehaviour
{
    [Tooltip("The canvas of the settings window UI.")]
    [SerializeField]
    private Canvas _SettingsWindowCanvas;

    [Header("Settings Panes")]

    [Tooltip("The gameplay settings pane.")]
    [SerializeField]
    private GameObject _GameplaySettingsPane;

    [Tooltip("The video settings pane.")]
    [SerializeField]
    private GameObject _VideoSettingsPane;

    [Tooltip("The audio settings pane.")]
    [SerializeField]
    private GameObject _AudioSettingsPane;



    void Awake()
    {
        Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        // Show the pause menu.
        _SettingsWindowCanvas.gameObject.SetActive(true);

        // Pause time.
        Time.timeScale = 0.0f;

    }

    public void Close()
    {
        // Resume time.
        Time.timeScale = 1.0f;

        // Hide the pause menu.
        _SettingsWindowCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Saves all settings.
    /// </summary>
    private void SaveSettings()
    {
        SaveGameplaySettings();
        SaveVideoSettings();
        SaveAudioSettings();
    }

    /// <summary>
    /// Saves gameplay settings.
    /// </summary>
    /// <remarks>
    /// This should save to Unity's PlayerPrefs, and also update any in-memory values to make the settings changes take effect.
    /// </remarks>
    private void SaveGameplaySettings()
    {

    }

    /// <summary>
    /// Saves video settings.
    /// </summary>
    /// <remarks>
    /// This should save to Unity's PlayerPrefs, and also update any in-memory values to make the settings changes take effect.
    /// </remarks>
    private void SaveVideoSettings()
    {

    }

    /// <summary>
    /// Saves audio settings.
    /// </summary>
    /// <remarks>
    /// This should save to Unity's PlayerPrefs, and also update any in-memory values to make the settings changes take effect.
    /// </remarks>
    private void SaveAudioSettings()
    {
        
    }

    private void HideAllSettingsPanes()
    {
        _GameplaySettingsPane.SetActive(false);
        _VideoSettingsPane.SetActive(false);
        _AudioSettingsPane.SetActive(false);
    }

    public void OnGameplaySettingsClicked()
    {
        HideAllSettingsPanes();

        _GameplaySettingsPane.SetActive(true);
    }

    public void OnVideoSettingsClicked()
    {
        HideAllSettingsPanes();

        _VideoSettingsPane.SetActive(true);
    }
    public void OnAudioSettingsClicked()
    {
        HideAllSettingsPanes();

        _AudioSettingsPane.SetActive(true);
    }

    public void OnExitClicked()
    {
        // Save the settings.
        SaveSettings();

        // Return to the previous game state (will be main menu or in game).
        GameManager.Instance.ReturnToPreviousState(false);

        // Close the settings window.
        Close();
    }

}
