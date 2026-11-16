using UnityEngine;

// Shade: walks between two points. When it sees the hero it stops, its eyes flash (wind-up),
// then it chases the hero. If it loses the hero it waits a moment and goes back to patrol.
// Touching it kills the hero (Hazard on the same object).
public class ShadeEnemy : MonoBehaviour
{
    // The numbers match the int parameter "State" in Shade.controller
    enum State { Patrol, Alert, Chase, Lost }

    // All balance numbers live in a data asset (Shade_Stats or Shade_Hard)
    public ShadeStats stats;

    [Header("Patrol (waypoints, set by the LevelLoader)")]
    public Transform pointA;
    public Transform pointB;

    [Header("Vision")]
    public Transform eye;
    public LayerMask wallLayer;

    [Header("Sound")]
    public AudioClip alertSound;     // growl at the start of the wind-up

    State state;
    float timer;
    int dir = 1;                     // 1 = looks right, -1 = looks left
    Transform target;                // the waypoint it walks to

    Collider2D hero;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Find the hero once and start walking to point B
    void Start()
    {
        hero = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = pointB;
        SetState(State.Patrol);
    }

    void Update()
    {
        // Freeze when the hero is dead
        if (GameManager.Instance.isDead) return;

        timer += Time.deltaTime;
        bool seesHero = EnemyVision.CanSee(eye.position, Vector2.right * dir, hero.bounds.center, stats.visionRange, stats.visionAngle, wallLayer);

        switch (state)
        {
            case State.Patrol:
                // Walk to the waypoint, then turn to the other one
                if (MoveTo(target.position.x, stats.patrolSpeed))
                    target = (target == pointA) ? pointB : pointA;
                if (seesHero) SetState(State.Alert);
                break;

            case State.Alert:
                // Stand still and look at the hero until the wind-up is over
                dir = hero.bounds.center.x > transform.position.x ? 1 : -1;
                spriteRenderer.flipX = dir < 0;
                if (timer >= stats.alertTime) SetState(State.Chase);
                break;

            case State.Chase:
                // Run after the hero, but never leave the patrol zone (so it never falls off)
                float minX = Mathf.Min(pointA.position.x, pointB.position.x);
                float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
                MoveTo(Mathf.Clamp(hero.bounds.center.x, minX, maxX), stats.chaseSpeed);
                if (!seesHero) SetState(State.Lost);
                break;

            case State.Lost:
                // Look around; chase again if the hero shows up, else go back to patrol
                if (seesHero) SetState(State.Chase);
                else if (timer >= stats.lostTime) SetState(State.Patrol);
                break;
        }
    }

    // Change the state, reset the timer, tell the Animator, play the wind-up sound
    void SetState(State newState)
    {
        state = newState;
        timer = 0f;
        animator.SetInteger("State", (int)newState);
        if (newState == State.Alert) AudioManager.Instance.PlaySfx(alertSound);
    }

    // Step towards x along the ground. Returns true when x is reached.
    bool MoveTo(float x, float speed)
    {
        Vector3 pos = transform.position;
        if (Mathf.Abs(x - pos.x) < 0.05f) return true;

        dir = x > pos.x ? 1 : -1;
        spriteRenderer.flipX = dir < 0;
        pos.x = Mathf.MoveTowards(pos.x, x, speed * Time.deltaTime);
        transform.position = pos;
        return false;
    }
}
