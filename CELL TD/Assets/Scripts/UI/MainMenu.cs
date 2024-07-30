using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class MainMenu : MonoBehaviour
{
    // Go To Map Selector
    public void GoToSelector()
    {
        GameManager.Instance.SetGameState(typeof(GameState_LevelSelect));
    }

    // Open Settings
    public void OpenSettings()
    {
        GameManager.Instance.SetGameState(typeof(GameState_Settings));
    }

    // Open Credits
    public void OpenCredits()
    {
        GameManager.Instance.SetGameState(typeof(GameState_Credits));
    }

    // Quit Game
    public void QuitGame()
    {
        Debug.Log("Exiting game...");

        GameManager.Instance.SetGameState(typeof(GameState_ShutDown));
    }
}
