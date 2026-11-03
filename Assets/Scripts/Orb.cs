using UnityEngine;

// A light orb: +10 score when the hero touches it.
public class Orb : MonoBehaviour
{
    // Tell the GameManager that this orb exists
    void Start()
    {
        GameManager.Instance.RegisterOrb();
    }

    // The hero picks the orb up
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddOrb();
            Destroy(gameObject);
        }
    }
}
