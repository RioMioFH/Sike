using System.Collections;
using UnityEngine;

public class PopUpSpikesTrap : MonoBehaviour
{
    // Reference to the spike object that appears temporarily
    [SerializeField] private GameObject popUpSpikes;

    // Duration in seconds the spikes stay visible
    [SerializeField] private float visibleTime = 1.0f;

    // Ensures the trap is triggered only once
    private bool triggered = false;

    // Vertical distance the spikes move when appearing
    [SerializeField] private float moveDistance = 0.5f;
     // Speed of the spike movement animation
    [SerializeField] private float moveSpeed = 10f;

    // Cached positions for hidden and visible spike states
    private Vector3 hiddenPosition;
    private Vector3 shownPosition;

    // Unity method called before Start()
    private void Awake()
    {   
        // Store the visible position of the spikes
        shownPosition = popUpSpikes.transform.position;

        // Calculate the hidden position below the visible position
        hiddenPosition = shownPosition - Vector3.up * moveDistance;

        // Move spikes to hidden position at scene start
        popUpSpikes.transform.position = hiddenPosition;
    }

    // Method to activate the pop-up spike trap
    public void Trigger()
    {
        // Prevent multiple triggers
        if (triggered) return;
            triggered = true;

        // Enable spike object
        popUpSpikes.SetActive(true);
   
        // Animate spikes moving upwards into view
        StartCoroutine(MoveSpikes(hiddenPosition, shownPosition));

        // Starts countdown to hide spikes again
        StartCoroutine(HideAfterDelay());
    }

     // Unity method called when another collider enters this trigger
    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Trigger the trap only when the player enters
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }   
    
    // Waits for a short time before hiding the spikes again
    private IEnumerator HideAfterDelay()
    {
        // Keep spikes acive for the defined duration
        yield return new WaitForSeconds(visibleTime);
        
         // Animate spikes moving back to the hidden position
        StartCoroutine(MoveSpikes(shownPosition, hiddenPosition));

        // Short delay to ensure movement finishes before disabling
        yield return new WaitForSeconds(0.2f);

        // Disable spike object after hiding
        popUpSpikes.SetActive(false);
    }  

    // Smoothly moves the spikes between two positions
    private IEnumerator MoveSpikes(Vector3 from, Vector3 to)
    {   
        // Progress value for the spike movement animation
        float moveProgress = 0f;

        // Move spikes smoothly from start to target position
        while (moveProgress < 1f)
        {   
            // Increase movement progress over time
            moveProgress += Time.deltaTime * moveSpeed;

            // Update spike position
            popUpSpikes.transform.position = Vector3.Lerp(from, to, moveProgress);
            
            // Wait for next frame
            yield return null;
        }
    }
}