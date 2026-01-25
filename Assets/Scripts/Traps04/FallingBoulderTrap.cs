using UnityEngine;

public class FallingBoulderTrap : MonoBehaviour
{
    // Reference to the boulder that will fall
    [SerializeField] private Rigidbody2D boulderRigidbody;

    // Ensures the trap is triggered only once
    private bool triggered = false;

    // Unity method called when another collider enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }

    // Activates the falling boulder
    private void Trigger()
    {
        triggered = true;

        // Enable physics so gravity makes the boulder fall
        boulderRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
}
