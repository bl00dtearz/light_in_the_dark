using UnityEngine;

// Shade: walks between two points. When it sees the hero it stops, its eyes flash (wind-up),
// then it chases the hero. If it loses the hero it waits a moment and goes back to patrol.
// Touching it kills the hero (Hazard on the same object).
public class ShadeEnemy : MonoBehaviour
{
    // The numbers match the int parameter "State" in Shade.controller
    enum State { Patrol, Alert, Chase, Lost }

    [Header("Patrol (waypoints)")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 1.5f;

    [Header("Vision")]
    public Transform eye;
    public float visionRange = 6f;
    public float visionAngle = 90f;
    public LayerMask wallLayer;

    [Header("Attack")]
    public float alertTime = 0.5f;   // wind-up: eyes flash before the chase
    public float chaseSpeed = 3.5f;
    public float lostTime = 1.5f;    // cooldown: waits before going back to patrol

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
        bool seesHero = EnemyVision.CanSee(eye.position, Vector2.right * dir, hero.bounds.center, visionRange, visionAngle, wallLayer);

        switch (state)
        {
            case State.Patrol:
                // Walk to the waypoint, then turn to the other one
                if (MoveTo(target.position.x, patrolSpeed))
                    target = (target == pointA) ? pointB : pointA;
                if (seesHero) SetState(State.Alert);
                break;

            case State.Alert:
                // Stand still and look at the hero until the wind-up is over
                dir = hero.bounds.center.x > transform.position.x ? 1 : -1;
                spriteRenderer.flipX = dir < 0;
                if (timer >= alertTime) SetState(State.Chase);
                break;

            case State.Chase:
                // Run after the hero, but never leave the patrol zone (so it never falls off)
                float minX = Mathf.Min(pointA.position.x, pointB.position.x);
                float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
                MoveTo(Mathf.Clamp(hero.bounds.center.x, minX, maxX), chaseSpeed);
                if (!seesHero) SetState(State.Lost);
                break;

            case State.Lost:
                // Look around; chase again if the hero shows up, else go back to patrol
                if (seesHero) SetState(State.Chase);
                else if (timer >= lostTime) SetState(State.Patrol);
                break;
        }
    }

    // Change the state, reset the timer and tell the Animator
    void SetState(State newState)
    {
        state = newState;
        timer = 0f;
        animator.SetInteger("State", (int)newState);
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
