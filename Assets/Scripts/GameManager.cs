using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static reference so other scripts can access the GameManager globally
    public static GameManager Instance {get; private set;}

    // Position where the player respawns (in Inspector)
    [SerializeField] private Transform spawnPoint;

    // Reference to game over UI shown when player dies
    [SerializeField] private GameOverUI gameOverUI;

    // Counts how many times the player has died in the current run
    public int DeathCount {get; private set;} = 0;
    
    // Total time played during the current run
    public float TimePlayed { get; private set; } = 0f;

    // Controls whether the game time is currently running
    private bool isTiming = false;

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
        // Do not count time outside of level scenes
        if (!isTiming) return;

        // Increase total play time independent of time scale
        TimePlayed += Time.unscaledDeltaTime;
    }

    // Called when the player dies
    public void ShowGameOver()
    {   
        // Increase death counter
        DeathCount++;

        Debug.Log("Player died. DeathCount = " + DeathCount);
        
        // Show game over screen
        gameOverUI.Show();
    }

    // Method that respawns the player after death
    public void RespawnPlayer (GameObject player)
    {
        // Get player physics component
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

        // Reset movement if Rigidbody exists
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector2.zero;
        }

        // Move player back to spawn position
        player.transform.position = spawnPoint.position;
    }

    // Called automatically when a new scene has finished loading
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   
        // Enable time tracking only in level scenes
        isTiming = scene.name.StartsWith("Level");

        // Find new spawn point in the loaded scene
        GameObject sp = GameObject.FindWithTag("SpawnPoint");
        if (sp != null) spawnPoint = sp.transform;

        // Find new game over UI in the loaded scene
        gameOverUI = FindAnyObjectByType<GameOverUI>();
    }

    // Resets all run-related values for a new game
    public void ResetRun()
    {
        DeathCount = 0;
        TimePlayed = 0f;
    }
}
