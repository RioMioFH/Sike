using System.Collections;
using UnityEngine;

public class DisappearTilemapOnTrigger : MonoBehaviour
{
    // Reference to the tilemap GameObject
    [SerializeField] private GameObject tilemapObject;

    // Delay before the tilemap disappears
    [SerializeField] private float disappearDelay = 0.3f;

    // Prevents triggering multiple times
    private bool triggered = false;

    // Triggered when the player enters the area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(DisappearRoutine());
        }
    }

    // Handles delayed tilemap disappearance
    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(disappearDelay);

        // Disable the entire tilemap (renderer + collider)
        tilemapObject.SetActive(false);
        // Remove trigger after activation
        gameObject.SetActive(false);
    }
}
