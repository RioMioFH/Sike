using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    // Loads the first level scene by name
    public void LoadLevel01()
    {
        SceneManager.LoadScene("Level_01");
    }

    // Loads the start screen from pause menu or end- winscreen e.g.
    public void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    
    }
    
    // Loads next level or EndScreen if last level
    public void LoadNextScene()
    {
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
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
