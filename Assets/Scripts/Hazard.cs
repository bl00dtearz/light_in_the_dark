using UnityEngine;

// Kills the hero on touch. Used on spikes and on the trigger under the pits.
public class Hazard : MonoBehaviour
{
    // The collider on this object is a trigger, so we get OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Die();
        }
    }
}
