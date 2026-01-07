using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player used for movement and jumping
    private Rigidbody2D playerRigidbody;
    // SpriteRenderer used for flipping the player left/right
    private SpriteRenderer spriteRenderer;

    // Horizontal movement speed
    [SerializeField] private float moveSpeed = 6f;
    // Jump force applied to the player
    [SerializeField] private float jumpSpeed = 12f;
    // Position used to check if the player is on the ground

    // Animator that controls player animations (idle, run, jump, death)
    [SerializeField] private Animator animator;

    // Positions used to check if the player is on the ground
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;

    // Radius of the ground check circle
    [SerializeField] private float groundCheckRadius = 0.15f;
    // Layer that represents the ground
    [SerializeField] private LayerMask groundLayer;

    // Upward impulse when the player dies (SuperMario-style)
    [SerializeField] private float deathJumpForce = 10f;
    // Delay before showing Game Over after death
    [SerializeField] private float deathDelay = 1.2f;
    // Prevents dying multiple times
    private bool isDead = false;

    // Time in seconds until the long idle animation triggers
    [SerializeField] private float longIdleTime = 3f;
    // Counts how long the player has been idle
    private float idleTimer = 0f;


    // Unity method called once at the start of the game
    void Start()
    {
        // Get Rigidbody2D component from the player object
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        // Get SpriteRenderer component for flipping the sprite
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Stop all player controls if the player is dead
        if (isDead) return;

        // Check if the player provides any movement or jump input
        bool hasInput = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // If any input is detected, reset idle timer and disable long idle animation
        if (hasInput)
        {
            idleTimer = 0f;
            animator.SetBool("isIdleTooLong", false);
        }
        else
        {   
            // Increase idle timer while no input is given
            idleTimer += Time.deltaTime;
            // Activate long idle animation after defined idle time
            if (idleTimer >= longIdleTime)
            {
                animator.SetBool("isIdleTooLong", true);
            }
        }

        // Read horizontal input (A/D or left/right arrows)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Apply horizontal movement
        playerRigidbody.linearVelocityX = horizontalInput * moveSpeed;

        // Set running animation state based on movement input
        if (horizontalInput != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Flip sprite depending on movement direction
        if (horizontalInput < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0f)
        {
            spriteRenderer.flipX = false;
        }

        // Check if any jump key was pressed
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        bool grounded = IsGrounded();

        // Jump only if jump key is pressed and player is on the ground
        if (jumpPressed && grounded)
        {
            playerRigidbody.linearVelocityY = jumpSpeed;
            animator.SetBool("isJumping", true);

        }

        // Reset jumping animation when player lands (or is falling down onto ground)
        if (grounded && playerRigidbody.linearVelocityY <= 0f)
        {
            animator.SetBool("isJumping", false);
        }
    }

    // Method that checks if the player is touching the ground
        private bool IsGrounded()
    {   
        // Overlap circles at both ground check positions to detect ground
        bool left = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, groundLayer);
        bool right = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, groundLayer);
        // Player is grounded if either check touches ground
        return left || right;
    }

    // Draws ground check circles in the Scene view for easier debugging
    private void OnDrawGizmos()
    {
        if (groundCheckLeft != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckLeft.position, groundCheckRadius);
        }

        if (groundCheckRight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckRight.position, groundCheckRadius);
        }
    }

    // Method that handles player death (animation + physics + game over
    public void Die()
    {   
        // Stop if the player already died
        if (isDead) return;
        isDead = true;

        // Set death animation state
        animator.SetBool("isDead", true);

        // Reset movement and apply upward death impulse (SuperMario-style)
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.gravityScale = 5f;
        playerRigidbody.AddForce(Vector2.up * deathJumpForce, ForceMode2D.Impulse);

        // Disable player collider so death does not trigger multiple times
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null) 
            collider.enabled = false;

        // Start delay before showing Game Over UI
        StartCoroutine(DeathRoutine());
    }

    // Coroutine that waits before triggering the Game Over UI
    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSecondsRealtime(deathDelay);
        GameManager.Instance.PlayerDied(gameObject);
    }     
}
