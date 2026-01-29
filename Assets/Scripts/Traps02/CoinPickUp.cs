using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    // Reference to the coin counter that triggers the trap
    [SerializeField] private CoinLastTrigger lastCoinTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore all objects that are not tagged as Player
        if (!other.CompareTag("Player"))
            return;

        // Register this coin pickup
        if (lastCoinTrigger != null)
            lastCoinTrigger.RegisterCoin();

        // Coin disappears
        Destroy(gameObject);
    }
}
