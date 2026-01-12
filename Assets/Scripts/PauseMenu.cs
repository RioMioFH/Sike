using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{   
    // UI panel that contains the pause menu
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    
    // Tracks whether the game is currently paused
    private bool isPaused = false;
    
    // Unity method called once on scene start
    void Start()
    {
        // Make sure pause UI starts hidden
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Ensure game runs normally on scene start
        Resume();   
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle pause state when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

     // Pauses gameplay and shows pause UI
    public void Pause()
    {
        // Mark game as paused
        isPaused = true;

        // Freeze game time
        Time.timeScale = 0f;

        // Stop run timer counting
        if (GameManager.Instance != null)
            GameManager.Instance.SetPaused(true);

        // Show pause panel
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    // Resumes gameplay and hides pause UI
    public void Resume()
    {   
        // Mark game as running
        isPaused = false;

        // Unfreeze game time
        Time.timeScale = 1f;

        // Continue run timer counting
        if (GameManager.Instance != null)
            GameManager.Instance.SetPaused(false);

        // Hide pause panel
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Placeholder for future settings menu
    public void OpenSettings()
    {
        Debug.Log("Settings menu not implemented yet.");
    }

    // Exits to start screen
    public void ExitToStartMenu()
    {
        // Ensure time scale is reset before leaving the level
        Time.timeScale = 1f;

        // Ensure game is not marked as paused
        if (GameManager.Instance != null)
            GameManager.Instance.SetPaused(false);

         // Load start screen scene
        SceneManager.LoadScene("StartScreen");
    }
}
