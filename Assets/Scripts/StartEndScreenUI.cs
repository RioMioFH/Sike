using UnityEngine;

public class StartEndScreenUI : MonoBehaviour
{
    // Called when the "Start Game" or "Play Again" button is pressed
    public void StartGame()
    {
        // Load first level and start a new run
        SceneLoader.Instance.LoadLevel01();
    }

    // Called when the "Main Menu" button is pressed (EndScreen only)
    public void LoadMainMenu()
    {
        // Load start screen
        SceneLoader.Instance.LoadStartScreen();
    }

    // Called when the "Exit Game" button is pressed (StartScreen only)
    public void ExitGame()
    {
        // Quit the game
        SceneLoader.Instance.QuitGame();
    }
}
