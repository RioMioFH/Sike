using UnityEngine;

public class CoinTrap : MonoBehaviour
{
    // Tag used to identify the player object
    [SerializeField] private string playerTag = "Player";

    // Reference to the trap script that should be triggered
    // (e.g. PopUpSpikesTrap, StartFloorTrap, MovingPlatform4)
    [SerializeField] private MonoBehaviour trapScript;

    // Name of the method that will be called on the trap script
    // Usually "Trigger" or "Activate"
    [SerializeField] private string methodName = "Trigger";

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

        // Invoke the specified trap method
        if (trapScript != null)
            trapScript.Invoke(methodName, 0f);
    }
}
