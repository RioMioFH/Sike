using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Unity method called when another object enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore all objects that are not tagged as Player
        if (!other.CompareTag("Player"))
            return;
        
        // Try to get the PlayerController component from the collided object
        PlayerController playerController = other.GetComponent<PlayerController>();
        
        // Trigger player death if a PlayerController was found
        if (playerController != null)
            playerController.Die();
    }
}