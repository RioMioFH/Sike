using System.Collections;
using UnityEngine;

public class MovingPlatform4 : MonoBehaviour
{
    [SerializeField] private float moveDistanceX = 2f;   // horizontal movement
    [SerializeField] private float moveDistanceY = 1f;   // vertical movement (down)
    [SerializeField] private float movingSpeed = 12f;     // fast movement speed (dash away)
    [SerializeField] private float returnSpeed = 3f;    // slow return speed
    [SerializeField] private Transform platform;
    private Vector3 startPosition;
    private bool triggered = false;

    private void Awake()
    {
        startPosition = platform.position;
    }

    public void Trigger()
    {   
        if (triggered) return;
        triggered = true;

        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        Vector3 targetPosition = startPosition 
        + Vector3.left * moveDistanceX 
        + Vector3.down * moveDistanceY;

        // Fast move away (player should fall)
        yield return StartCoroutine(MoveToPosition(targetPosition, movingSpeed));

        // Short pause (optional, makes the effect clearer)
        yield return new WaitForSeconds(1f);

        // Slow return to start position
        yield return StartCoroutine(MoveToPosition(startPosition, returnSpeed));
    }

    private IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        while (Vector3.Distance(platform.position, target) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, target, speed * Time.deltaTime);

            yield return null;
        }

        platform.position = target;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Trigger();
        }
    }
}