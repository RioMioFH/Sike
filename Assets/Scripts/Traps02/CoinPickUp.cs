using Unity.VisualScripting;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore all objects that are not tagged as Player
        if (!other.CompareTag("Player"))
            return;

        //Coins Disappear
        Destroy(gameObject);

    }

    
}
