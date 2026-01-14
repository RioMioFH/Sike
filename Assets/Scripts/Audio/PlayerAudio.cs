using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip deathSfx;
    [SerializeField] private AudioClip footstepSfx;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    // Plays jump sound effect
    public void PlayJump()
    {
        // Do nothing if AudioManager is missing
        if (AudioManager.Instance == null) return;

        // Play jump sound effect
        AudioManager.Instance.PlaySFX(jumpSfx);
    }

    // Plays death sound effect
    public void PlayDeath()
    {
        // Do nothing if AudioManager is missing
        if (AudioManager.Instance == null) return;

        // Play death sound effect
        AudioManager.Instance.PlaySFX(deathSfx);
    }

    // Plays footstep sound (called by animation events)
    public void PlayFootstep()
    {
        // Do nothing if references are missing
        if (playerController == null) return;

        // Do nothing if player is not on the ground
        if (!playerController.IsGrounded()) return;

        // Play footstep sound effect
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(footstepSfx);
    }
}
