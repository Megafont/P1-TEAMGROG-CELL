using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player enters the settings.
/// </summary>
public class GameState_Credits : GameState_Base
{
    public static bool IsLaunchingFromMainMenu = true;



    public GameState_Credits(GameManager parent)
        : base(parent)
    {

    }

    public override void OnEnter()
    {
        SceneManager.LoadScene("Credits");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
