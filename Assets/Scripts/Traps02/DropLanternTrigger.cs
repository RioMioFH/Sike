using UnityEngine;

public class DropLanternTrigger : MonoBehaviour
{
    // Tag that identifies the player object
    [SerializeField] private string playerTag = "Player";

    // Rigidbody2D of the lantern that should fall when the trap is triggered
    [SerializeField] private Rigidbody2D lanternRb;

    // Ensures the trap is triggered only once
    private bool triggered = false;

    // Called automatically when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent multiple activations
        if (triggered) return;

        // Only react when the player enters the trigger
        if (!other.CompareTag(playerTag)) return;

        // Mark trap as triggered
        triggered = true;

        // Switch lantern from kinematic to dynamic so it falls down
        if (lanternRb != null)
        {
            lanternRb.bodyType = RigidbodyType2D.Dynamic;
            lanternRb.WakeUp();
        }
    }
}
