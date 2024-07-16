using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class VictoryScreen : MonoBehaviour
{
    public static VictoryScreen Instance;


    void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogWarning("There is already a VictoryScreen instance in this scene. Self destructing...");
            Destroy(gameObject);
            return;
        }


        Instance = this;
        Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }   

    public void Open()
    {
        gameObject.SetActive(true);

        GameManager.Instance.SetGameState(typeof(GameState_Victory));
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public static void Show()
    {
        Instance.Open();
    }

    public static void Hide()
    {
        Instance.Close();
    }


    public void NextLevelClicked()
    {
        // Get the current level's name.
        string currentLevelName = SceneManager.GetActiveScene().name;

        // Get the level number that appears after the _ in the level name.
        int currentLevelNumber = int.Parse(currentLevelName.Split('_')[1]);

        // Get the next level's name
        string nextLevelName = $"Level_{currentLevelNumber + 1}";

        // Check if the next level exists.
        if (!SceneManager.GetSceneByName(nextLevelName).IsValid())
            throw new System.Exception($"Cannot go to the next level, because the scene \"{nextLevelName}\" does not exist!");


        // Load the next level.
        SceneManager.LoadScene(nextLevelName);

        Hide();
    }

    public void LevelSelectionClicked()
    {
        // Load the level selection screen.
        GameManager.Instance.SetGameState(typeof(GameState_LevelSelect));

        Hide();
    }

    public void ReturnToMainMenuClicked()
    {
        // Load the main menu.
        GameManager.Instance.SetGameState(typeof(GameState_MainMenu));

        Hide();
    }
}
