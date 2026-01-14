using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Global access to AudioManager
    public static AudioManager Instance { get; private set; }

    // Mixer reference (for Music/SFX volume control)
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    // Audio sources for musiC, sound effects and UI
    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;
    private void Awake()
    {   
        Debug.Log("AudioManager Awake ID=" + gameObject.GetInstanceID());
        // Ensure only one AudioManager exists
        if (Instance != null && Instance != this)
        {   
            Debug.Log("AudioManager DESTROY duplicate ID=" + gameObject.GetInstanceID());
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep AudioManager when scenes change
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
{
    Debug.Log("AudioManager OnDestroy ID=" + gameObject.GetInstanceID());
}

    // Plays background music (looped)
    public void PlayMusic(AudioClip clip)
    {
        // Do nothing if no clip was assigned
        if (clip == null) return;

        // Avoid restarting the same track
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        // Assign clip and play it as looping music
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

   // Plays a sound effect (interrupts previous SFX)
    public void PlaySFX(AudioClip clip)
    {
        // Do nothing if no clip was assigned
        if (clip == null) return;

        // Assign clip to SFX source (overwrites any currently playing SFX)
        sfxSource.clip = clip;

        // Play sound effect immediately
        sfxSource.Play();
    }

    // Plays a UI sound effect (does not interrupt gameplay SFX)
    public void PlayUISFX(AudioClip clip)
    {
        // Do nothing if no clip was assigned
        if (clip == null) return;
        
        // Play UI sound effect without stopping other UI sounds
        uiSource.PlayOneShot(clip);
    }
}
