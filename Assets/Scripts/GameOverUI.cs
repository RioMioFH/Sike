using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Panel that contains the Game Over UI
    [SerializeField] private GameObject gameOverPanel;

    // Checks if Game Over screen is active
    private bool isGameOverActive;

    // Method to ensure panel is hidden at scene start
    private void Start()
    {   
        isGameOverActive = false;
        gameOverPanel.SetActive(false);
    }
    private void Update()
    {
    // Restart level on any key while Game Over screen is active
    if (isGameOverActive && Input.anyKeyDown)
        {
            RestartLevel();
        }
    }

    // Method to show Game Over screen
    public void Show()
    {
        isGameOverActive = true;
        gameOverPanel.SetActive(true);
    }

    // Method to hide Game Over screen
    public void Hide()
    {
        isGameOverActive = false;
        gameOverPanel.SetActive(false);
    }

    // Method to reload current scene
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
