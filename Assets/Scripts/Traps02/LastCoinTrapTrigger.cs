using UnityEngine;

public class LastCoinTrapTrigger : MonoBehaviour
{
    // Reference to the PopUpSpikesTrap that should be triggered
    // when the player collects the last coin
    [SerializeField] private PopUpSpikesTrap spikesTrap;

    // Ensures the trap is triggered only once
    private bool triggered = false;

    // Called automatically when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent multiple activations
        if (triggered) return;

        // Only react when the player enters the trigger
        if (!other.CompareTag("Player")) return;

        // Mark trap as triggered
        triggered = true;

        // Trigger the spike trap
        if (spikesTrap != null)
            spikesTrap.Trigger();
    }
}
