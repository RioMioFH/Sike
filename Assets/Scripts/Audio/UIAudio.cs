using UnityEngine;

public class UIAudio : MonoBehaviour
{   
    // Sound effect played when a button is pressed
    [SerializeField] private AudioClip clickSfx;
    // Sound effect played when the level is completed
    [SerializeField] private AudioClip levelCompleteSfx;

    public void PlayClick()
    {   
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayUISFX(clickSfx);
    }

    // Plays level complete sound effect
    public void PlayLevelComplete()
    {
        // Do nothing if AudioManager is missing
        if (AudioManager.Instance == null) return;

        // Play level complete UI sound effect
        AudioManager.Instance.PlayUISFX(levelCompleteSfx);
    }
}
