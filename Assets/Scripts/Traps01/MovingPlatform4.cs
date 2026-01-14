using System.Collections;
using UnityEngine;

public class MovingPlatform4 : MonoBehaviour
{   
    // Horizontal movement distance of the platform
    [SerializeField] private float moveDistanceX = 2f;
    // Vertical movement distance of the platform (downwards)
    [SerializeField] private float moveDistanceY = 1f;
    // Speed used when the platform quickly moves away
    [SerializeField] private float movingSpeed = 12f;
    // Speed used when the platform slowly returns to its start position
    [SerializeField] private float returnSpeed = 3f;

    // Reference to the actual platform transform that moves
    [SerializeField] private Transform platform;

    // Initial position of the platform
    private Vector3 startPosition;
     // Ensures the platform is triggered only once
    private bool triggered = false;

    // Unity method called before Start()
    private void Awake()
    {   
        // Store the starting position of the platform
        startPosition = platform.position;
    }

    // Public method to trigger the platform movement
    public void Trigger()
    {   
        // Prevent multiple triggers
        if (triggered) return;
        triggered = true;

        // Start platform movement sequence
        StartCoroutine(MovePlatform());
    }

    // Handles the complete movement sequence of the platform
    private IEnumerator MovePlatform()
    {   
        // Calculate the target position the platform moves to
        Vector3 targetPosition = startPosition 
        + Vector3.left * moveDistanceX 
        + Vector3.down * moveDistanceY;

        // Fast move away so the player falls down
        yield return StartCoroutine(MoveToPosition(targetPosition, movingSpeed));

        // Short pause before returning (optional, improves readability)
        yield return new WaitForSeconds(1f);

        // Slowly return platform back to its starting position
        yield return StartCoroutine(MoveToPosition(startPosition, returnSpeed));
    }

    // Moves the platform smoothly to a target position
    private IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        // Move the platform until it is close enough to the target
        while (Vector3.Distance(platform.position, target) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, target, speed * Time.deltaTime);

            // Wait for next frame
            yield return null;
        }

        // Snap exactly to the target position
        platform.position = target;
    }

    // Unity method called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Trigger platform movement only when the player enters
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }
}