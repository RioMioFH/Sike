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

    // Names of the exposed AudioMixer parameters
    private const string MIX_MASTER = "MasterVol";
    private const string MIX_MUSIC  = "MusicVol";
    private const string MIX_SFX    = "SfxVol";
    private const string MIX_UISFX  = "UISfxVol";

    private void Awake()
    {   
        // Ensure only one AudioManager exists
        if (Instance != null && Instance != this)
        {   
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep AudioManager when scenes change
        DontDestroyOnLoad(gameObject);

        // Apply saved music volume at startup
        ApplyVolumes();
    }

    // Unity method called once after all Awake() calls in the scene
    private void Start()
    {
        // Apply saved volumes again here to make sure SettingsManager is ready
        ApplyVolumes();
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
    public void PlayUISFX(AudioClip clip, float volumeScale = 1f)
    {
        // Do nothing if no clip was assigned
        if (clip == null) return;

        // Clamp volumeScale so it can't go out of a reasonable range
        volumeScale = Mathf.Clamp(volumeScale, 0f, 4f);

        // Play UI sound effect with individual volume scaling
        uiSource.PlayOneShot(clip, volumeScale);
    }

    // Plays a sound effect without interrupting other SFX (useful for footsteps)
    public void PlaySFXOneShot(AudioClip clip, float volumeScale = 1f)
    {
        // Do nothing if no clip was assigned
        if (clip == null) return;

        // Clamp volumeScale to keep values reasonable
        volumeScale = Mathf.Clamp(volumeScale, 0f, 4f);

        // Play sound effect without stopping other SFX
        sfxSource.PlayOneShot(clip, volumeScale);
    }

    // Applies all saved volume settings from SettingsManager to the AudioMixer
    public void ApplyVolumes()
    {
        // Do nothing if mixer was not assigned
        if (mixer == null) return;

        // Do nothing if SettingsManager is not available
        if (SettingsManager.Instance == null) return;

        // Get volumes from settings (0.0-1.0)
        float masterVolume = SettingsManager.Instance.MasterVolume;
        float musicVolume  = SettingsManager.Instance.MusicVolume;
        float sfxVolume    = SettingsManager.Instance.SfxVolume;

        // Clamp values to avoid log(0)
        masterVolume = Mathf.Clamp(masterVolume, 0.01f, 1f);
        musicVolume  = Mathf.Clamp(musicVolume,  0.01f, 1f);
        sfxVolume    = Mathf.Clamp(sfxVolume,    0.01f, 1f);

        // Convert values to decibel for AudioMixer
        float masterDb = Mathf.Log10(masterVolume) * 20f;
        float musicDb  = Mathf.Log10(musicVolume)  * 20f;
        float sfxDb    = Mathf.Log10(sfxVolume)    * 20f;

        // Apply values to AudioMixer
        mixer.SetFloat(MIX_MASTER, masterDb);
        mixer.SetFloat(MIX_MUSIC,  musicDb);

        // SFX slider controls both SFX and UI_SFX
        mixer.SetFloat(MIX_SFX,    sfxDb);
        mixer.SetFloat(MIX_UISFX,  sfxDb);
    }
}