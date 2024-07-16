using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This state is when the player enters the level selector.
/// </summary>
public class GameState_LevelSelect : GameState_Base
{
    public GameState_LevelSelect(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
