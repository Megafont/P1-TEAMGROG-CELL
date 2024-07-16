using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;


    void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogWarning("There is already a GameOverScreen instance in this scene. Self destructing...");
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

        GameManager.Instance.SetGameState(typeof(GameState_GameOver));
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


    public void RetryClicked()
    {
        // Reload the current level.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
