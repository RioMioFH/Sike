using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    // Reference to the settings panel that gets shown/hidden
    [SerializeField] private GameObject settingsPanel;

    // Menu panel that should be hidden when settings opens (StartMenu or PausePanel)
    [SerializeField] private GameObject menuToHide;

    [Header("Audio Sliders (0 - 100)")]
    // Slider for master volume
    [SerializeField] private Slider masterSlider;

    // Slider for music volume
    [SerializeField] private Slider musicSlider;

    // Slider for sfx volume
    [SerializeField] private Slider sfxSlider;

    [Header("UI Toggles")]
    // Toggle for showing time in HUD
    [SerializeField] private Toggle showTimeToggle;

    // Toggle for showing deaths in HUD
    [SerializeField] private Toggle showDeathsToggle;

    [Header("Buttons")]
    // Button to reset all settings
    [SerializeField] private Button resetButton;

    // Prevents UI callbacks while we are filling in values
    private bool isInitializing = false;

    // Tracks whether the settings menu is currently open
    private bool isOpen = false;


    // Called once on scene start
    private void Start()
    {
        // If no panel was assigned, use this GameObject
        if (settingsPanel == null)
            settingsPanel = gameObject;

        // Hide settings menu at scene start
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        // Close settings menu when Escape is pressed
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    // Opens the settings menu
    public void Open()
    {
        // Hide the menu behind settings (if assigned)
        if (menuToHide != null)
            menuToHide.SetActive(false);

        // Show the settings panel
        settingsPanel.SetActive(true);

        // Sync UI with current settings values
        RefreshUIFromSettings();

        // Mark settings as open
        isOpen = true;
    }   

    // Closes the settings menu
    public void Close()
    {
        // Hide the settings panel
        settingsPanel.SetActive(false);

        // Show the menu behind settings again (if assigned)
        if (menuToHide != null)
            menuToHide.SetActive(true);
        
        // Mark settings as closed
        isOpen = false;
    }

        // Updates sliders/toggles to match the current values in SettingsManager
    private void RefreshUIFromSettings()
    {
        // Do nothing if SettingsManager is missing
        if (SettingsManager.Instance == null) return;

        // Block callbacks while we set UI values
        isInitializing = true;

        // Audio Sliders convert 0-1 values into 0-100
        if (masterSlider != null)
            masterSlider.value = SettingsManager.Instance.MasterVolume * 100f;

        if (musicSlider != null)
            musicSlider.value = SettingsManager.Instance.MusicVolume * 100f;

        if (sfxSlider != null)
            sfxSlider.value = SettingsManager.Instance.SfxVolume * 100f;

        // UI Toggles directly match bool values
        if (showTimeToggle != null)
            showTimeToggle.isOn = SettingsManager.Instance.ShowTime;

        if (showDeathsToggle != null)
            showDeathsToggle.isOn = SettingsManager.Instance.ShowDeaths;

        // Allow callbacks again
        isInitializing = false;
    }

    // Called when the Master slider value changes (0-100)
    public void OnMasterSliderChanged(float value)
    {
        // Ignore events while we are setting UI values
        if (isInitializing) return;

        // Convert 0–100 (UI) to 0–1 (settings)
        float value01 = value / 100f;

        // Apply master volume
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetMasterVolume(value01);

            // Apply mixer volumes immediately so the change is audible right away
            if (AudioManager.Instance != null)
                AudioManager.Instance.ApplyVolumes();
        }
    }

    // Called when the Music slider value changes (0–100)
    public void OnMusicSliderChanged(float value)
    {
        // Ignore events while we are setting UI values
        if (isInitializing) return;

        // Convert 0–100 (UI) to 0–1 (settings)
        float value01 = value / 100f;

        // Apply music volume
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetMusicVolume(value01);

            // Apply mixer volumes immediately so the change is audible right away
            if (AudioManager.Instance != null)
                AudioManager.Instance.ApplyVolumes();
        }
    }

    // Called when the SFX slider value changes (0–100)
    public void OnSfxSliderChanged(float value)
    {
        // Ignore events while we are setting UI values
        if (isInitializing) return;

        // Convert 0–100 (UI) to 0–1 (settings)
        float value01 = value / 100f;

        // Apply SFX volume
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.SetSfxVolume(value01);

            // Apply mixer volumes immediately (SFX slider also controls UI_SFX)
            if (AudioManager.Instance != null)
                AudioManager.Instance.ApplyVolumes();
        }
    }

    // Called when the Show Time toggle value changes
    public void OnShowTimeToggleChanged(bool value)
    {   
        // Ignore events while we are setting UI values
        if (isInitializing) return;

        // Apply UI setting
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetShowTime(value);
    }

    // Called when the Show Deaths toggle value changes
    public void OnShowDeathsToggleChanged(bool value)
    {
        // Ignore events while we are setting UI values
        if (isInitializing) return;

        // Apply UI setting
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.SetShowDeaths(value);
    }

    // Called when the Reset button is clicked
    public void OnResetClicked()
    {
        // Reset all settings to default values
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.ResetToDefaults();

        // Update UI to show the default values
        RefreshUIFromSettings();

        // Apply mixer volumes after reset (RefreshUI blocks callbacks with isInitializing)
        if (AudioManager.Instance != null)
            AudioManager.Instance.ApplyVolumes();
    }
}