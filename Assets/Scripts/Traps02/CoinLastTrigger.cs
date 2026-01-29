using UnityEngine;

public class CoinLastTrigger : MonoBehaviour
{
    // How many coins are needed to trigger the trap
    [SerializeField] private int coinsNeeded = 3;

    // Trap that will be triggered when the last coin is collected
    [SerializeField] private PopUpSpikesTrap spikesTrap;

    // Counts collected coins for this scene
    private int coinsCollected = 0;

    // Called by CoinPickUp when a coin is collected
    public void RegisterCoin()
    {
        coinsCollected++;

        // Trigger spikes only when the last coin is collected
        if (coinsCollected >= coinsNeeded)
        {
            if (spikesTrap != null)
                spikesTrap.Trigger();
        }
    }
}
