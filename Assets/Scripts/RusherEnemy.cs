using UnityEngine;

// Rusher: waits in a corridor. When the hero is in front of it, it shakes (wind-up),
// then dashes in a straight line. After the dash it is stunned (cooldown) and walks back.
// The hero survives by jumping over it. Touching it kills the hero (Hazard on the same object).
public class RusherEnemy : MonoBehaviour
{
    // The numbers match the int parameter "State" in Rusher.controller
    enum State { Idle, WindUp, Charge, Stunned, Return }

    // All balance numbers live in a data asset (Rusher_Stats or Rusher_Hard)
    public RusherStats stats;

    [Header("Vision")]
    public Transform eye;
    public int facing = -1;              // -1 = looks left, 1 = looks right (set by the LevelLoader)
    public LayerMask wallLayer;

    [Header("Sound")]
    public AudioClip windUpSound;        // snort + rumble at the start of the wind-up
    public AudioClip crashSound;         // hit at the end of the dash

    [Header("Effects")]
    public ParticleSystem crashFx;       // sparks and stones from the wall
    public float crashShake = 0.1f;

    State state;
    float timer;
    float startX;
    float chargeStartX;
    Collider2D hero;
    Animator animator;
    SpriteRenderer spriteRenderer;
    SpriteFx spriteFx;

    // Remember the start point and find the hero once
    void Start()
    {
        hero = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteFx = GetComponent<SpriteFx>();
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
                if (EnemyVision.CanSee(eye.position, Vector2.right * facing, hero.bounds.center, stats.visionRange, stats.visionAngle, wallLayer))
                    SetState(State.WindUp);
                break;

            case State.WindUp:
                if (timer >= stats.windUpTime)
                {
                    chargeStartX = transform.position.x;
                    SetState(State.Charge);
                }
                break;

            case State.Charge:
                // Dash forward until a wall is right in front or the dash is long enough
                transform.position += Vector3.right * facing * stats.chargeSpeed * Time.deltaTime;
                bool wallAhead = Physics2D.Raycast(eye.position, Vector2.right * facing, stats.wallStopDistance, wallLayer);
                bool tooFar = Mathf.Abs(transform.position.x - chargeStartX) >= stats.maxChargeDistance;
                if (wallAhead || tooFar) SetState(State.Stunned);
                break;

            case State.Stunned:
                if (timer >= stats.stunTime)
                {
                    spriteRenderer.flipX = facing > 0;   // turn around to walk back
                    SetState(State.Return);
                }
                break;

            case State.Return:
                // Walk back to the start point, then look forward again
                Vector3 pos = transform.position;
                pos.x = Mathf.MoveTowards(pos.x, startX, stats.returnSpeed * Time.deltaTime);
                transform.position = pos;
                if (Mathf.Abs(pos.x - startX) < 0.01f)
                {
                    spriteRenderer.flipX = facing < 0;
                    SetState(State.Idle);
                }
                break;
        }
    }

    // Change the state, reset the timer, tell the Animator, play the sound
    void SetState(State newState)
    {
        state = newState;
        timer = 0f;
        animator.SetInteger("State", (int)newState);
        if (newState == State.WindUp) AudioManager.Instance.PlaySfx(windUpSound);
        if (newState == State.Stunned)
        {
            AudioManager.Instance.PlaySfx(crashSound);
            Juice.Instance.Spawn(crashFx, eye.position);
            Juice.Instance.Shake(0.2f, crashShake);
            spriteFx.Flash();
        }
    }
}
