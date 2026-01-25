using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{   
    // Reference to the trigger collider of this object
    private BoxCollider2D triggerCollider;

    // Prevents triggering the level end multiple times
    private bool triggered = false;

    // Name of the animator trigger that starts the lay down animation
    [SerializeField] private string layDownTriggerName = "LayDown";

    // Level complete sound that is played when the player fully enters the trigger
    [SerializeField] private AudioClip levelEndSfx;


    // Called once when the object is created even before Start()
    private void Awake()
    {   
        // Get the BoxCollider2D of this game object
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    // Called every frame while another collider stays inside this trigger
    private void OnTriggerStay2D(Collider2D other)
    {   
        // Do nothing if already triggered
        if (triggered) return;

        // Only trigger if object is player
        if (!other.CompareTag("Player"))
            return;

        // Bounds of player and trigger
        Bounds playerBounds = other.bounds;
        Bounds triggerBounds = triggerCollider.bounds;

        // Check if player is fully inside trigger
        if (triggerBounds.Contains(playerBounds.min) && triggerBounds.Contains(playerBounds.max))
        {   
            // Mark level end as triggered
            triggered = true;

            // Play level end sound immediately
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFXOneShot(levelEndSfx, 4f);

            // Disable player movement immediately
            other.GetComponent<PlayerController>()?.DisableMovement();

            // Play lay down animation once
            Animator animator = other.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger(layDownTriggerName);

            // Notify GameManager that the level was completed
            GameManager.Instance.LevelCompleted();
        }
    }
}