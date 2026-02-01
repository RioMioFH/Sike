using UnityEngine;

public class SikeSoundTrigger : MonoBehaviour
{
    // SIKE sound that is played when the player enters this trigger
    [SerializeField] private AudioClip sikeSfx;

    // Prevents the sound from playing multiple times
    private bool played = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the sound was already played, do nothing
        if (played)
            return;

        // Only react to the player
        if (!other.CompareTag("Player"))
            return;

        // Play SIKE sound via AudioManager
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXOneShot(sikeSfx, 1f);

        // Mark as played so it can't be triggered again
        played = true;
    }
}
