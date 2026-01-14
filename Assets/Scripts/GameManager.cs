using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static reference so other scripts can access the GameManager globally
    public static GameManager Instance { get; private set; }

    // Position where the player respawns
    private Transform spawnPoint;

    // Reference to game over UI shown when player dies
    private GameOverUI gameOverUI;

    // Delay before loading the next scene after level completion
    [SerializeField] private float levelCompleteDelay = 0.6f;

    // Prevents triggering the level end multiple times
    private bool levelCompleted = false;

    // Counts how many times the player has died in the current run
    public int DeathCount { get; private set; } = 0;

    // Total time played during the current run
    public float TimePlayed { get; private set; } = 0f;

    // Controls whether the game time is currently running
    private bool isTiming = false;

    // Tracks whether the game is currently paused
    public bool IsPaused { get; private set; } = false;

    // Unity method called once when the scene loads (before Start)
    private void Awake()
    {
        // Make sure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            // Another GameManager was found, remove it
            Destroy(gameObject);
            return;
        }

        // Store this GameManager so other scripts can use it
        Instance = this;

        // Keep GameManager when scenes reload
        DontDestroyOnLoad(gameObject);
    }

    // Register scene load event
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unregister scene load event
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    private void Update()
    {
        // Do not count time outside of level scenes or while paused
        if (!isTiming || IsPaused) return;

        // Increase total play time independent of time scale
        TimePlayed += Time.unscaledDeltaTime;
    }

    // Called when the player dies
    public void ShowGameOver()
    {
        // Increase death counter
        DeathCount++;
        Debug.Log("Player died. DeathCount = " + DeathCount);

        // Show game over screen (if available)
        if (gameOverUI != null)
            gameOverUI.Show();
        else
            Debug.LogWarning("GameOverUI not found in this scene.");
    }

    // Method that respawns the player after death
    public void RespawnPlayer(GameObject player)
    {
        // Stop if no player was given
        if (player == null) return;

        // Get player physics component
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        // Reset movement if Rigidbody exists
        if (playerRigidbody != null)
            playerRigidbody.linearVelocity = Vector2.zero;

        // Move player back to spawn position (if available)
        if (spawnPoint != null)
            player.transform.position = spawnPoint.position;
        else
            Debug.LogWarning("SpawnPoint not found in this scene.");
    }

    // Called when the player completes the level
    public void LevelCompleted()
    {
        // Do nothing if already completed
        if (levelCompleted) return;
        levelCompleted = true;

        // Trigger UI level complete sound (no clip stored here)
        FindAnyObjectByType<UIAudio>()?.PlayLevelComplete();

        // Load next scene after a short delay
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    // Resets all run-related values for a new game
    public void ResetRun()
    {
        DeathCount = 0;
        TimePlayed = 0f;
    }

    // Sets paused state for time tracking
    public void SetPaused(bool paused)
    {
        IsPaused = paused;
    }

    // Called automatically when a new scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset level completed flag for the new scene
        levelCompleted = false;

        // Enable time tracking only in level scenes
        isTiming = scene.name.StartsWith("Level");

        // Refresh scene references (spawn point, UI)
        RefreshSceneReferences();
    }

    // Finds and updates all scene references needed by the GameManager
    private void RefreshSceneReferences()
    {
        // Find new spawn point in the loaded scene
        GameObject sp = GameObject.FindWithTag("SpawnPoint");
        spawnPoint = sp != null ? sp.transform : null;

        // Find new game over UI in the loaded scene
        gameOverUI = FindAnyObjectByType<GameOverUI>();
    }

    // Loads the next scene after a short delay
    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait briefly before loading the next scene
        yield return new WaitForSeconds(levelCompleteDelay);

        // Find SceneLoader in this scene and load next scene
        SceneLoader loader = FindAnyObjectByType<SceneLoader>();
        if (loader != null)
            loader.LoadNextScene();
        else
            Debug.LogWarning("SceneLoader not found in this scene.");
    }
}
