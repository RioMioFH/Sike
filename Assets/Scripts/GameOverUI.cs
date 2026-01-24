using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Panel that contains the Game Over UI
    [SerializeField] private GameObject gameOverPanel;

    // Checks if Game Over screen is active
    private bool isGameOverActive;
    // Stores a delayed show request while paused
    private bool pendingShow;

    // Method to ensure panel is hidden at scene start
    private void Start()
    {    
        Hide();
    }

    // Update is called once per frame
    private void Update()
    {   
        // Show game over after resume if it was delayed
        if (pendingShow && (GameManager.Instance == null || !GameManager.Instance.IsPaused))
        {
            Show();
            return;
        }
        
        // Do nothing while game is paused
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
        return;
        
        // Restart level on any key except Escape while Game Over screen is active
        if (isGameOverActive && Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            RestartLevel();
        }
    }

    // Method to show Game Over screen
    public void Show()
    {   
        // Do not show game over while game is paused
        if (GameManager.Instance != null && GameManager.Instance.IsPaused)
        {
            pendingShow = true;
            return;
        }
        
        isGameOverActive = true;
        pendingShow = false;
        gameOverPanel.SetActive(true);
    }

    // Method to hide Game Over screen
    public void Hide()
    {
        isGameOverActive = false;
        pendingShow = false;
        gameOverPanel.SetActive(false);
    }

    // Method to reload current scene
    private void RestartLevel()
    {   
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
