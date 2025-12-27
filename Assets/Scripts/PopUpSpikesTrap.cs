using System.Collections;
using UnityEngine;

public class PopUpSpikesTrap : MonoBehaviour
{
    // Reference to the spike tilemap that appears temporarily
    [SerializeField] private GameObject popUpSpikes;

    // Duration in seconds how long the spikes stay visible
    [SerializeField] private float visibleTime = 1.0f;

    // Ensures the trap is only triggered once
    private bool triggered = false;

    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 hiddenPosition;
    private Vector3 shownPosition;


    // Method to activate the pop-up spike trap
    public void Trigger()
    {
        // Stop if the trap has already been triggered
        if (triggered) return;
        triggered = true;

        // Spikes become visible
        popUpSpikes.SetActive(true);

        StartCoroutine(MoveSpikes(hiddenPosition, shownPosition));

        // Starts countdown to hide spikes again
        StartCoroutine(HideAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        // Triggers the trap when player passes through the trigger zone
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }   

    private void Awake()
    {
        shownPosition = popUpSpikes.transform.position;
        hiddenPosition = shownPosition - Vector3.up * moveDistance;

        popUpSpikes.transform.position = hiddenPosition;
    }

    private IEnumerator HideAfterDelay()
    {
        // Keep spikes acive for the defined duration
        yield return new WaitForSeconds(visibleTime);
        
        // Hide spikes and disable their kill zone
        StartCoroutine(MoveSpikes(shownPosition, hiddenPosition));
        yield return new WaitForSeconds(0.2f);
        popUpSpikes.SetActive(false);
    }  

    private IEnumerator MoveSpikes(Vector3 from, Vector3 to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            popUpSpikes.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }

}

