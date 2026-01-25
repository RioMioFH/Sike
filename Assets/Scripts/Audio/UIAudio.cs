using UnityEngine;

public class UIAudio : MonoBehaviour
{   
    // Sound effect played when a button is pressed
    [SerializeField] private AudioClip clickSfx;
    
    // Plays button click sound effect
    public void PlayClick()
    {   
        // If Audiomanager is not missing play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayUISFX(clickSfx, 0.5f);
    }
}
