using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{      
    // Static reference so other scripts can access the SettingsManager globally
    public static SettingsManager Instance { get; private set; }

    // Keys used to save and load settings from PlayerPrefs
    private const string KEY_MASTER = "settings_master";
    private const string KEY_MUSIC  = "settings_music";
    private const string KEY_SFX    = "settings_sfx";
    private const string KEY_SHOW_TIME   = "settings_show_time";
    private const string KEY_SHOW_DEATHS = "settings_show_deaths";

    // Default values used for reset
    private const float DEFAULT_MASTER = 0.5f;
    private const float DEFAULT_MUSIC  = 0.5f;
    private const float DEFAULT_SFX    = 0.5f;

    private const bool DEFAULT_SHOW_TIME   = true;
    private const bool DEFAULT_SHOW_DEATHS = true;


    [Header("Audio Settings ((0.0-1.0))")]
    // Master volume (controls everything)
    public float MasterVolume { get; private set; } = 0.5f;

    // Music volume (background music)
    public float MusicVolume { get; private set; } = 0.5f;

    // SFX volume (sound effects)
    public float SfxVolume { get; private set; } = 0.5f;

    [Header("UI Settings")]
    // Show / hide the time display in the HUD
    public bool ShowTime { get; private set; } = true;

    // Show / hide the death counter in the HUD
    public bool ShowDeaths { get; private set; } = true;

    // Loads all settings from PlayerPrefs (or keeps defaults if nothing is saved)
    private void LoadSettings()
    {
        // Load audio settings (float values)
        MasterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(KEY_MASTER, MasterVolume));
        MusicVolume  = Mathf.Clamp01(PlayerPrefs.GetFloat(KEY_MUSIC,  MusicVolume));
        SfxVolume    = Mathf.Clamp01(PlayerPrefs.GetFloat(KEY_SFX,    SfxVolume));

        // Load UI settings (stored as 0 or 1)
        ShowTime   = PlayerPrefs.GetInt(KEY_SHOW_TIME,   ShowTime ? 1 : 0) == 1;
        ShowDeaths = PlayerPrefs.GetInt(KEY_SHOW_DEATHS, ShowDeaths ? 1 : 0) == 1;
    }

    // Saves all current settings to PlayerPrefs
    private void SaveSettings()
    {
        // Save audio settings
        PlayerPrefs.SetFloat(KEY_MASTER, MasterVolume);
        PlayerPrefs.SetFloat(KEY_MUSIC,  MusicVolume);
        PlayerPrefs.SetFloat(KEY_SFX,    SfxVolume);

        // Save UI settings (bool stored as 0 or 1)
        PlayerPrefs.SetInt(KEY_SHOW_TIME,   ShowTime ? 1 : 0);
        PlayerPrefs.SetInt(KEY_SHOW_DEATHS, ShowDeaths ? 1 : 0);

        // Force PlayerPrefs to write data to disk
        PlayerPrefs.Save();
    }

    // Called once when the object is created even before Start()
    private void Awake()
    {   
        // Make sure only one SettingsManager exists
        if (Instance != null && Instance != this)
        {
            // Another SettingsManager was found, remove it
            Destroy(gameObject);
            return;
        }

        // Store this SettingsManager so other scripts can use it
        Instance = this;

        // Load saved settings (or defaults)
        LoadSettings();

        // Keep SettingsManager when scenes reload
        DontDestroyOnLoad(gameObject);
    }

    // Sets the master volume (0.0-1.0)
    public void SetMasterVolume(float value)
    {
        // Clamp value to (0.0-1.0) so it can never go out of range
        MasterVolume = Mathf.Clamp01(value);

        // Save change
        SaveSettings();
    }

    // Sets the music volume (0.0-1.0)
    public void SetMusicVolume(float value)
    {
        // Clamp value to (0.0-1.0) so it can never go out of range
        MusicVolume = Mathf.Clamp01(value);

        // Save change
        SaveSettings();
    }

    // Sets the sfx volume (0.0-1.0)
    public void SetSfxVolume(float value)
    {
        // Clamp value to (0.0-1.0) so it can never go out of range
        SfxVolume = Mathf.Clamp01(value);

        // Save change
        SaveSettings();
    }

    // Enables or disables the time display
    public void SetShowTime(bool show)
    {
        ShowTime = show;

        // Save change
        SaveSettings();
    }

    // Enables or disables the death counter display
    public void SetShowDeaths(bool show)
    {
        ShowDeaths = show;

        // Save change
        SaveSettings();
    }

    // Resets all settings back to default values
    public void ResetToDefaults()
    {
        // Reset audio
        SetMasterVolume(DEFAULT_MASTER);
        SetMusicVolume(DEFAULT_MUSIC);
        SetSfxVolume(DEFAULT_SFX);

        // Reset UI
        SetShowTime(DEFAULT_SHOW_TIME);
        SetShowDeaths(DEFAULT_SHOW_DEATHS);
    }
}
