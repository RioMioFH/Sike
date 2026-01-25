using UnityEngine;

public class MushroomBounceTrap : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Header("Launch")]
    [SerializeField] private float launchVelocity = 22f; // THIS is what you tune

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Force a strong, consistent upward launch
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, launchVelocity);
    }
}
