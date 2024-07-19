using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Selector : MonoBehaviour
{
    public void ReturnToMenu()
    {
        GameManager.Instance.SetGameState(typeof(GameState_MainMenu));
    }
}
