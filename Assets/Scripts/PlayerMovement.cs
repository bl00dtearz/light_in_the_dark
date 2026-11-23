using UnityEngine;
using UnityEngine.InputSystem;

// Moves the hero left and right, makes him jump and sends Speed and Grounded to the Animator.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // Speed and jump come from a data asset (PlayerStats)
    public PlayerStats stats;

    // An empty child at the hero's feet and the layer the hero can stand on
    public Transform groundCheck;
    public float groundRadius = 0.12f;
    public LayerMask groundLayer;

    // Sound
    public AudioClip jumpSound;

    // Dust under the feet when the hero lands
    public ParticleSystem landFx;

    // Components on the same GameObject
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Actions from the project-wide Input System asset (keyboard and on-screen buttons)
    InputAction moveAction;
    InputAction jumpAction;

    bool grounded;
    bool wasGrounded = true;

    // Find the components and the input actions once
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // Do nothing while the game is paused
        if (Time.timeScale == 0f) return;

        // A dead hero freezes and turns red
        if (GameManager.Instance.isDead)
        {
            rb.simulated = false;
            animator.speed = 0f;
            spriteRenderer.color = new Color(1f, 0.35f, 0.35f);
            return;
        }

        // Is there ground under the feet?
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if (grounded && !wasGrounded) Juice.Instance.Spawn(landFx, groundCheck.position);
        wasGrounded = grounded;

        // Walk: -1 (left) .. 1 (right) from A/D, the arrows or the on-screen buttons
        float moveX = moveAction.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(moveX * stats.moveSpeed, rb.linearVelocity.y);

        // Jump only when standing on the ground
        if (jumpAction.WasPressedThisFrame() && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            AudioManager.Instance.PlaySfx(jumpSound);
        }

        // Face the walking direction
        if (moveX > 0.01f) spriteRenderer.flipX = false;
        else if (moveX < -0.01f) spriteRenderer.flipX = true;

        // Tell the Animator what the hero is doing
        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("Grounded", grounded);
    }
}
