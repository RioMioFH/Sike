using System.Collections;
using UnityEngine;

public class HorizontalSpikesTrap : MonoBehaviour
{
    // Reference to the spike object that moves horizontally
    [SerializeField] private Transform spikes;

    // Horizontal movement distance from the start position
    [SerializeField] private float moveDistance = 5f;

    // Speed of the spike movement
    [SerializeField] private float moveSpeed = 3f;

    // Optional pause time at each end of the movement
    [SerializeField] private float pauseTime = 0.5f;

    // Ensures the movement loop is started only once
    private bool activated = false;
    
    // Movement direction (1 = right, -1 = left)
    [SerializeField] private int direction = -1;

    // Cached movement positions
    private Vector3 startPosition;
    private Vector3 endPosition;

    // Unity method called before Start()
    private void Awake()
    {
        // Store initial spike position
        startPosition = spikes.position;

        // Calculate end position to the right
        endPosition = startPosition + Vector3.right * moveDistance * direction;

    }

    // Unity method called when another collider enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start movement only when player enters
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(MoveLoop());
        }
    }

    // Continuously moves spikes back and forth forever
    private IEnumerator MoveLoop()
    {
        while (true)
        {
            // Move from start to end
            yield return StartCoroutine(MoveToPosition(startPosition, endPosition));

            // Optional pause at the end
            yield return new WaitForSeconds(pauseTime);

            // Move back from end to start
            yield return StartCoroutine(MoveToPosition(endPosition, startPosition));

            // Optional pause at the start
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Smoothly moves spikes between two positions
    private IEnumerator MoveToPosition(Vector3 from, Vector3 to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            spikes.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        // Snap exactly to target position
        spikes.position = to;
    }
}
