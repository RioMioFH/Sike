using System.Collections;
using UnityEngine;

public class MushroomLaunchTrap : MonoBehaviour
{
    // Visual object that moves up/down
    [SerializeField] private Transform mushroomVisual;

    // How far the mushroom pops up from the hidden position
    [SerializeField] private float popUpDistance = 0.5f;

    // How fast the mushroom pops up
    [SerializeField] private float popUpDuration = 0.08f;

    // How long the mushroom stays up before going down
    [SerializeField] private float stayUpTime = 0.15f;

    // How fast the mushroom goes back down
    [SerializeField] private float goDownDuration = 0.12f;

    // Upward launch velocity applied to the player
    [SerializeField] private float launchVelocity = 12f;

    // Prevents retriggering while animating
    private bool isActive = false;

    // Cached local positions
    private Vector3 hiddenLocalPos;
    private Vector3 shownLocalPos;

    private void Awake()
    {
        // Cache only the hidden start position
        if (mushroomVisual != null)
            hiddenLocalPos = mushroomVisual.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (!other.CompareTag("Player"))
            return;

        // Prevent multiple triggers while active
        if (isActive)
            return;

        // Launch the player upwards
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchVelocity);

        // Recalculate shown position using current popUpDistance
        shownLocalPos = hiddenLocalPos + Vector3.up * popUpDistance;

        // Pop up the mushroom visual
        if (mushroomVisual != null)
            StartCoroutine(PopUpRoutine());
    }

    private IEnumerator PopUpRoutine()
    {
        isActive = true;

        // Move up
        yield return MoveLocal(hiddenLocalPos, shownLocalPos, popUpDuration);

        // Stay up briefly
        if (stayUpTime > 0f)
            yield return new WaitForSeconds(stayUpTime);

        // Move down
        yield return MoveLocal(shownLocalPos, hiddenLocalPos, goDownDuration);

        isActive = false;
    }

    private IEnumerator MoveLocal(Vector3 from, Vector3 to, float duration)
    {
        // Instant move if duration is zero
        if (duration <= 0f)
        {
            mushroomVisual.localPosition = to;
            yield break;
        }

        float t = 0f;

        // Smooth movement over time
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            mushroomVisual.localPosition = Vector3.Lerp(from, to, lerp);
            yield return null;
        }

        mushroomVisual.localPosition = to;
    }
}
