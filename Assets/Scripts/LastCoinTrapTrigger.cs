using UnityEngine;

public class LastCoinTrapTrigger : MonoBehaviour
{
    [SerializeField] private PopUpSpikesTrap spikesTrap;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (spikesTrap != null)
            spikesTrap.Trigger();
    }
}
