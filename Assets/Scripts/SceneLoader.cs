using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
// Static reference so other scripts can access the SceneLoader globally
    public static SceneLoader Instance { get; private set; }

    public void Awake()
    {
        // Make sure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            // Another GameManager was found, remove it
            Destroy(gameObject);
            return;
        }

        // Keep SceneLoader when scenes reload
        DontDestroyOnLoad(gameObject);
    }

    // Loads the first level scene and starts a new run
    public void LoadLevel01()
    {   
        // Reset run values before starting the level  
        if (GameManager.Instance != null)
            GameManager.Instance.ResetRun();

        // Load first level scene
        SceneManager.LoadScene("Level_01");
    }

    // Loads the start screen from pause menu or from endscreen e.g.
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }
    
    // Loads the next level or the end screen if no next level exists
    public void LoadNextScene()
    {   
        // Get name of the currently active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reset run when starting again from end screen
        if (currentSceneName == "EndScreen")
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ResetRun();
        }

        // Get build index of the current scene
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // If next scene exists in build profile then load it, otherwise go to EndScreen
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    // Exits the game by quitting the application
    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}