using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelector : MonoBehaviour
{
    [SerializeField]
    private string _mapName;
    public void LoadMap()
    {
        Debug.Log("1");
        // Set the new current level number.
        GameManager.Instance.CurrentLevelNumber = 1;

        // Manually switch to the in-game state.
        GameManager.Instance.SetGameState(typeof(GameState_InGame));
    }
}
