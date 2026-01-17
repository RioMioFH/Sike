using UnityEngine;

public class UIAudio : MonoBehaviour
{   
    // Sound effect played when a button is pressed
    [SerializeField] private AudioClip clickSfx;
    // Sound effect played when the level is completed
    [SerializeField] private AudioClip levelCompleteSfx;

    // Plays button click sound effect
    public void PlayClick()
    {   
        // If Audiomanager is not missing play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayUISFX(clickSfx, 0.5f);
    }

    // Plays level complete sound effect
    public void PlayLevelComplete()
    {
        // If Audiomanager is not missing play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayUISFX(levelCompleteSfx, 4f);
    }
}
