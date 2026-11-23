using UnityEngine;

// A floor block that looks normal but falls a moment after the hero touches it.
[RequireComponent(typeof(Rigidbody2D))]
public class FakeFloor : MonoBehaviour
{
    // Delay, gravity and lifetime come from a data asset (GameRules)
    public GameRules rules;
    public AudioClip crackSound;

    // Stone crumbs and a small shake when the block falls
    public ParticleSystem fallFx;
    public float fallShake = 0.05f;

    Rigidbody2D rb;
    bool triggered;

    // The Rigidbody2D starts as Kinematic, so the block hangs in the air
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // The hero touched the block: start the timer once
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered && collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            AudioManager.Instance.PlaySfx(crackSound);
            Invoke(nameof(Fall), rules.fakeFloorDelay);
        }
    }

    // Switch to Dynamic so gravity pulls the block down, then remove it
    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Juice.Instance.Spawn(fallFx, transform.position);
        Juice.Instance.Shake(0.15f, fallShake);
        rb.gravityScale = rules.fakeFloorGravity;
        Destroy(gameObject, rules.fakeFloorLifetime);
    }
}
