using UnityEngine;

// Watcher: a floating eye that does not move. When it sees the hero it opens,
// aims a red line at him (wind-up), shoots one light bolt and rests (cooldown).
// Walls block its view, so the hero can hide behind them.
public class WatcherEnemy : MonoBehaviour
{
    // The numbers match the int parameter "State" in Watcher.controller
    enum State { Sleep, Aim, Shoot, Cooldown }

    // All balance numbers live in a data asset (Watcher_Stats)
    public WatcherStats stats;

    [Header("Vision")]
    public Transform eye;
    public LayerMask wallLayer;

    [Header("Attack")]
    public Projectile projectilePrefab;
    public LineRenderer aimLine;

    [Header("Sound")]
    public AudioClip aimSound;         // rising hum during the wind-up
    public AudioClip shootSound;

    State state;
    float timer;
    Collider2D hero;
    Animator animator;

    // Find the hero once and start asleep
    void Start()
    {
        hero = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        SetState(State.Sleep);
    }

    void Update()
    {
        // Freeze when the hero is dead
        if (GameManager.Instance.isDead)
        {
            aimLine.enabled = false;
            return;
        }

        timer += Time.deltaTime;
        bool seesHero = EnemyVision.CanSee(eye.position, Vector2.right, hero.bounds.center, stats.visionRange, stats.visionAngle, wallLayer);

        switch (state)
        {
            case State.Sleep:
                if (seesHero) SetState(State.Aim);
                break;

            case State.Aim:
                // The red line follows the hero; if he hides, the eye goes back to sleep
                aimLine.SetPosition(0, eye.position);
                aimLine.SetPosition(1, hero.bounds.center);
                if (!seesHero) SetState(State.Sleep);
                else if (timer >= stats.aimTime) Shoot();
                break;

            case State.Shoot:
                if (timer >= stats.shootTime) SetState(State.Cooldown);
                break;

            case State.Cooldown:
                if (timer >= stats.cooldownTime) SetState(seesHero ? State.Aim : State.Sleep);
                break;
        }
    }

    // Fire one bolt to where the aim line points now
    void Shoot()
    {
        Vector2 direction = ((Vector2)hero.bounds.center - (Vector2)eye.position).normalized;
        Projectile bolt = Instantiate(projectilePrefab, eye.position, Quaternion.identity);
        bolt.Launch(direction, stats.boltSpeed, stats.boltLifetime);
        SetState(State.Shoot);
    }

    // Change the state, reset the timer, show the line only while aiming, tell the Animator, play the sound
    void SetState(State newState)
    {
        state = newState;
        timer = 0f;
        aimLine.enabled = newState == State.Aim;
        animator.SetInteger("State", (int)newState);
        if (newState == State.Aim) AudioManager.Instance.PlaySfx(aimSound);
        if (newState == State.Shoot) AudioManager.Instance.PlaySfx(shootSound);
    }
}
