using UnityEngine;

// A light bolt shot by the Watcher: flies straight, kills the hero, breaks on walls.
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifeTime = 4f;
    public LayerMask wallLayer;

    // Called by the Watcher right after it creates the bolt
    public void Launch(Vector2 direction, float speed)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
        Destroy(gameObject, lifeTime);
    }

    // Hit the hero: he dies. Hit a wall (Ground layer): the bolt disappears.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Die();
            Destroy(gameObject);
        }
        else if ((wallLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}
