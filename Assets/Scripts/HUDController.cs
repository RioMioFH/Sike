using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{   
    // UI text references for displaying time and deaths
    [Header ("UI References")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text deathText;

    // Update is called once per frame
    private void Update()
    {   
        // Do nothing if GameManager is not available
        if (GameManager.Instance == null) return;  
        
        // Get total time played from GameManager
        float time = GameManager.Instance.TimePlayed;

        // Convert time into minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // Update time display if reference exists
        if (timeText != null)
        timeText.text = $"Time: {minutes:00}:{seconds:00}";

        // Update death counter display if reference exists
        if (deathText != null) 
        deathText.text = $"Deaths: {GameManager.Instance.DeathCount}";
    }
}
