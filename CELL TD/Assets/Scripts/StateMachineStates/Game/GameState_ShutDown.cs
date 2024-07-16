using System.Collections;
using System.Collections.Generic;

using UnityEngine;


/// <summary>
/// This state is for initialization when the game first starts up.
/// </summary>
public class GameState_ShutDown : GameState_Base
{
    public GameState_ShutDown(GameManager parent)
        : base(parent)
    {

    }


    public override void OnEnter()
    {
        // Do shutdown work.
        ShutDown.DoShutDownWork();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (ShutDown.ShutDownIsComplete)
        {
            Application.Quit();
        }
    }

}
