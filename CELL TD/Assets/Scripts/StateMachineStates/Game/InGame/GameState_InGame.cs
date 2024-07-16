using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player is in the game, and not paused.
/// </summary>
public class GameState_InGame : GameState_Base
{
    public static bool IsReturningFromSettings = false;



    public GameState_InGame(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        // We don't want to reload the level if we are just returning from the Settings dialog.
        if (!IsReturningFromSettings)
        {
            SceneManager.LoadScene($"Level_{GameManager.Instance.CurrentLevelNumber}");
        }


        // Reset this flag variable.
        IsReturningFromSettings = false;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
