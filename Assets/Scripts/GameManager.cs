using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static reference so other scripts can access the GameManager globally
    public static GameManager Instance;

    // Position where the player respawns (in Inspector)
    [SerializeField] private Transform spawnPoint;

    // Reference to game over UI shown when player dies
    [SerializeField] private GameOverUI gameOverUI;

    // Counts how many times the player has died
    private int deathCount = 0;

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
    }

    public void PlayerDied(GameObject player)
    {
        // Show game over screen
        gameOverUI.Show();
    }

    // Method that respawns the player after death
    public void RespawnPlayer (GameObject player)
    {
        // Increase death counter (used later for UI or stats)
        deathCount++;

        // Console output just for testing
        Debug.Log("Respawn! Deathcount = " + deathCount);

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
}
