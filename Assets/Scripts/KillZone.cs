using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Unity method called when another object enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered is the player
        if (other.CompareTag("Player"))
        {
            // Tell the GameManager to respawn the player
            GameManager.Instance.RespawnPlayer(other.gameObject);
        }
    }
}
