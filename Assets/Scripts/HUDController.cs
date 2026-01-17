using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{   
    // UI text references for displaying time and deaths
    [Header ("UI References")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text deathText;

    // Stores last HUD visibility state to avoid calling SetActive every frame
    private bool lastShowTime;
    private bool lastShowDeaths;

    // Unity method called once on scene start
    private void Start()
    {
        // Set initial state so Update applies it once
        lastShowTime = true;
        lastShowDeaths = true;
    }


    // Update is called once per frame
    private void Update()
    {   
        // Do nothing if GameManager is not available
        if (GameManager.Instance == null) return;

        // Do nothing if SettingsManager is not available
        if (SettingsManager.Instance == null) return;  
        
        // Read current settings
        bool showTime = SettingsManager.Instance.ShowTime;
        bool showDeaths = SettingsManager.Instance.ShowDeaths;
        
        // Update HUD visibility based on the REAL active state (more robust than lastShow flags)
if (timeText != null && timeText.gameObject.activeSelf != showTime)
{
    // Show/hide the time UI element based on the setting
    timeText.gameObject.SetActive(showTime);
}

if (deathText != null && deathText.gameObject.activeSelf != showDeaths)
{
    // Show/hide the death UI element based on the setting
    deathText.gameObject.SetActive(showDeaths);
}
        
        // Get total time played from GameManager
        float time = GameManager.Instance.TimePlayed;

        // Convert time into minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // Update time display only if visible and reference exists
        if (timeText != null && showTime)
            timeText.text = $"Time: {minutes:00}:{seconds:00}";

        // Update death counter only if visible and reference exists
        if (deathText != null && showDeaths)
            deathText.text = $"Deaths: {GameManager.Instance.DeathCount}";
    }
}
