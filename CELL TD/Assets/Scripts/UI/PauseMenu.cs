using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("The canvas of the pause menu UI.")]
    [SerializeField]
    private Canvas _PauseMenuCanvas;



    private void Awake()
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
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_PauseMenuCanvas.gameObject.activeSelf)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    public void Open()
    {
        // Show the pause menu.
        _PauseMenuCanvas.gameObject.SetActive(true); 
        
        // Pause time.
        Time.timeScale = 0.0f;

    }

    public void Close()
    {
        // Resume time.
        Time.timeScale = 1.0f;

        // Hide the pause menu.
        _PauseMenuCanvas.gameObject.SetActive(false);
    }

    public void OnResumeClicked()
    {
        Close();
    }

    public void OnRestartLevelClicked()
    {
        Time.timeScale = 1.0f;

        // GameManager.CurrentLevelNumber is still this level's number, so this works fine.
        GameManager.Instance.SetGameState(typeof(GameState_InGame), true);
    }

    public void OnLevelSelectionClicked()
    {
        Time.timeScale = 1.0f;

        GameManager.Instance.SetGameState(typeof(GameState_LevelSelect));
    }

    public void OnSettingsClicked()
    {
        GameManager.Instance.SetGameState(typeof(GameState_Settings));
    }

    public void OnReturnToMainMenuClicked()
    {
        Time.timeScale = 1.0f;

        GameManager.Instance.SetGameState(typeof(GameState_MainMenu));
    }

    public void OnExitGameClicked()
    {
        Debug.Log("Exiting game...");

        GameManager.Instance.SetGameState(typeof(GameState_ShutDown));
    }


}
