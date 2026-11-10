using UnityEngine;

// Rusher: waits in a corridor. When the hero is in front of it, it shakes (wind-up),
// then dashes in a straight line. After the dash it is stunned (cooldown) and walks back.
// The hero survives by jumping over it. Touching it kills the hero (Hazard on the same object).
public class RusherEnemy : MonoBehaviour
{
    // The numbers match the int parameter "State" in Rusher.controller
    enum State { Idle, WindUp, Charge, Stunned, Return }

    [Header("Vision")]
    public Transform eye;
    public int facing = -1;              // -1 = looks left, 1 = looks right
    public float visionRange = 9f;
    public float visionAngle = 20f;      // narrow: only sees straight ahead
    public LayerMask wallLayer;

    [Header("Attack")]
    public float windUpTime = 0.7f;      // wind-up: shakes with red eyes
    public float chargeSpeed = 10f;
    public float maxChargeDistance = 12f;
    public float wallStopDistance = 0.6f;
    public float stunTime = 1.5f;        // cooldown after the dash
    public float returnSpeed = 2f;

    State state;
    float timer;
    float startX;
    float chargeStartX;
    Collider2D hero;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Remember the start point and find the hero once
    void Start()
    {
        hero = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startX = transform.position.x;
        spriteRenderer.flipX = facing < 0;
        SetState(State.Idle);
    }

    void Update()
    {
        // Freeze when the hero is dead
        if (GameManager.Instance.isDead) return;

        timer += Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                if (EnemyVision.CanSee(eye.position, Vector2.right * facing, hero.bounds.center, visionRange, visionAngle, wallLayer))
                    SetState(State.WindUp);
                break;

            case State.WindUp:
                if (timer >= windUpTime)
                {
                    chargeStartX = transform.position.x;
                    SetState(State.Charge);
                }
                break;

            case State.Charge:
                // Dash forward until a wall is right in front or the dash is long enough
                transform.position += Vector3.right * facing * chargeSpeed * Time.deltaTime;
                bool wallAhead = Physics2D.Raycast(eye.position, Vector2.right * facing, wallStopDistance, wallLayer);
                bool tooFar = Mathf.Abs(transform.position.x - chargeStartX) >= maxChargeDistance;
                if (wallAhead || tooFar) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer >= stunTime)
                {
                    spriteRenderer.flipX = facing > 0;   // turn around to walk back
                    SetState(State.Return);
                }
                break;

            case State.Return:
                // Walk back to the start point, then look forward again
                Vector3 pos = transform.position;
                pos.x = Mathf.MoveTowards(pos.x, startX, returnSpeed * Time.deltaTime);
                transform.position = pos;
                if (Mathf.Abs(pos.x - startX) < 0.01f)
                {
                    spriteRenderer.flipX = facing < 0;
                    SetState(State.Idle);
                }
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
}
