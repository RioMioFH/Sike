using UnityEngine;

public class StartEndScreenUI : MonoBehaviour
{
    // Called when the "Start Game" or "Play Again" button is pressed
    public void StartGame()
    {
        // Make sure SceneLoader exists
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("SceneLoader Instance not found.");
            return;
        }

        // Load first level and start a new run
        SceneLoader.Instance.LoadLevel01();
    }

    // Called when the "Main Menu" button is pressed (EndScreen only)
    public void LoadMainMenu()
    {
        // Make sure SceneLoader exists
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("SceneLoader Instance not found.");
            return;
        }

        // Load start screen
        SceneLoader.Instance.LoadStartScreen();
    }

    // Called when the "Exit Game" button is pressed (StartScreen only)
    public void ExitGame()
    {
        // Make sure SceneLoader exists
        if (SceneLoader.Instance == null)
        {
            Debug.LogError("SceneLoader Instance not found.");
            return;
        }

        // Quit the game
        SceneLoader.Instance.QuitGame();
    }
}
