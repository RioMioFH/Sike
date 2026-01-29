using UnityEngine;

public class MushroomBounceTrap : MonoBehaviour
{
    [Header("Launch")]
    // Vertical velocity applied to the player when bouncing on the mushroom
    // This value controls how high the player is launched
    [SerializeField] private float launchVelocity = 22f;

    // Called automatically when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react when the player enters the trigger
        if (!other.CompareTag("Player")) return;

        // Get the player's Rigidbody2D
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Apply a strong, consistent upward launch while keeping horizontal movement
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchVelocity);
    }
}
