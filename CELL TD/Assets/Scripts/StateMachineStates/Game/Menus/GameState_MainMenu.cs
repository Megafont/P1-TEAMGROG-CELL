using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class GameState_MainMenu : GameState_Base
{
    public GameState_MainMenu(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {

    }
}
