using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player enters the settings.
/// </summary>
public class GameState_Settings : GameState_Base
{
    public static bool IsLaunchingFromMainMenu = true;


    private SettingsWindow SettingsWindowInstance;



    public GameState_Settings(GameManager parent)
        : base(parent)
    {

    }

    public override void OnEnter()
    {
        // Display the settings window
        GameObject.FindAnyObjectByType<SettingsWindow>().Open();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
