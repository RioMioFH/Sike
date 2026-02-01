using UnityEngine;

public class LanternImpact : MonoBehaviour
{
    // Reference to the lantern Rigidbody
    [SerializeField] private Rigidbody2D rb;

     // Trigger collider that kills the player while falling
    [SerializeField] private Collider2D killTrigger;

    // Horizontal impulse applied after hitting the player
    [SerializeField] private float sideBounceForce = 5f;

    // Small upward impulse applied together with the side bounce
    [SerializeField] private float upBounceForce = 2f;

    // Rotational impulse applied on impact
    [SerializeField] private float spinTorque = 4f;
  
    private void Awake()
    {
        // Automatically assign Rigidbody if not set in Inspector
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        if (killTrigger == null)
            killTrigger = GetComponent<Collider2D>();
    }

    // Called when the lantern collides with another collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Disable killing after colliding with any non-player object
        if (!other.CompareTag("Player"))
            return;

        // Kill the player without the Mario-style upward launch
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
            player.Die(false);

        // Bounce the lantern sideways and slightly upwards
        float direction = Random.value < 0.5f ? -1f : 1f;
        rb.AddForce(new Vector2(direction * sideBounceForce, upBounceForce), ForceMode2D.Impulse);

        // Apply rotation so the bounce feels dynamic
        rb.AddTorque(spinTorque, ForceMode2D.Impulse);

        // Disable kill trigger after first hit
        killTrigger.enabled = false;
    }

    // Handle physical collisions (ground contact)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Disable kill trigger once the lantern touches the ground
        if (killTrigger.enabled)
            killTrigger.enabled = false;
    }
}
