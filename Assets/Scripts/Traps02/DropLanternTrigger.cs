using UnityEngine;

public class DropLanternTrigger : MonoBehaviour
{   
    // Reference to the lantern Rigidbody
    [SerializeField] private Rigidbody2D lanternRb;

    // Initial downward speed when the lantern is released
    [SerializeField] private float initialDownSpeed = 8f;

    // Prevents the lantern from being triggered multiple times
    private bool triggered = false;

    // Called when the player enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Ensure the trigger is only activated once
        if (triggered) return;

        // Only react to the player
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Safety check in case the reference is missing
        if (lanternRb == null) return;

        // Switch the lantern to dynamic so gravity and collisions apply
        lanternRb.bodyType = RigidbodyType2D.Dynamic;

        // Make sure the rigidbody is active in the physics simulation
        lanternRb.WakeUp();

        // Apply an initial downward velocity to make the fall feel heavier
        lanternRb.linearVelocity = new Vector2(0f, -initialDownSpeed);
    }
}
