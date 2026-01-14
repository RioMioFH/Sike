using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // Music clip to play in this scene
    [SerializeField] private AudioClip music;

    private void Start()
    {
        // Start background music via AudioManager
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic(music);
    }
}