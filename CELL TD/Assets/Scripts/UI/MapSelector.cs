using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;



/// <summary>
/// This enum lists all the levels.
/// </summary>
/// <remarks>
/// The names in this enum MUST match the scene names, as this is how it knows which sceen to load.
/// It will take the name of the enum value, and prepend it with "Level_" to get the name of the scene
/// to load for the specified level.
/// 
/// When a new level is added to the game, just add its name in this enum and you will be able to
/// select it on a level panel in the level select screen once you've created a new one for that level.
/// </remarks>
public enum Levels
{
    BloodVeins,
    Mouth,
}


/// <summary>
/// This class is the functionality for each level panel on the level select screen.
/// </summary>
public class MapSelector : MonoBehaviour
{
    [Description("Sets which level is loaded when the player clicks the button on this level selector panel.")]
    [SerializeField]
    private Levels _Level;
    

    /// <summary>
    /// Loads the level specified in the _Level field using the Unity inspector.
    /// </summary>
    public void LoadMap()
    {
        switch (_Level)
        {

        }

        // Set the new current level number.
        GameManager.Instance.CurrentLevelName = Enum.GetName(typeof(Levels), _Level);

        // Manually switch to the in-game state.
        GameManager.Instance.SetGameState(typeof(GameState_InGame));
    }
}
