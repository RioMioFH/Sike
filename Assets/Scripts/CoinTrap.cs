using UnityEngine;

public class CoinTrap : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private MonoBehaviour trapScript;
    [SerializeField] private string methodName = "Trigger";

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;

        if (trapScript != null)
            trapScript.Invoke(methodName, 0f);
    }
}
