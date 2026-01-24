using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player used for movement and jumping
    private Rigidbody2D playerRigidbody;
    // SpriteRenderer used for flipping the player left/right
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    // Horizontal movement speed
    [SerializeField] private float moveSpeed = 6f;
    // Jump force applied to the player
    [SerializeField] private float jumpSpeed = 12f;
     // Prevents movement when the level is completed
    private bool movementEnabled = true;

    [Header("Animation")]
    // Animator that controls player animations (idle, run, jump, death)
    [SerializeField] private Animator animator;

     [Header("Ground Check")]
    // Positions used to check if the player is on the ground
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;
    // Radius of the ground check circle
    [SerializeField] private float groundCheckRadius = 0.15f;
    // Layer that represents the ground
    [SerializeField] private LayerMask groundLayer;

    [Header("Death")]
    // Upward impulse when the player dies (SuperMario-style)
    [SerializeField] private float deathJumpForce = 10f;
    // Delay before showing Game Over after death
    [SerializeField] private float deathDelay = 1.2f;
    // Prevents dying multiple times
    private bool isDead = false;

    [Header("Idle")]
    // Time in seconds until the long idle animation triggers
    [SerializeField] private float longIdleTime = 3f;
    // Counts how long the player has been idle
    private float idleTimer = 0f;

    // Cached input values
    private float horizontalInput;
    private bool jumpPressed;
    private bool grounded;

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
        // Stop all player controls if player is dead or game is paused
        if (isDead || !movementEnabled || (GameManager.Instance != null && GameManager.Instance.IsPaused)) return;
        
        ReadInput();

        // Check if the player is on the ground
        grounded = IsGrounded();

        Idle();
        Run();
        Jump();
    }   

    // Reads movement and jump input once per frame
    private void ReadInput()
    {
        // Read horizontal input (A/D or left/right arrows)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Check if any jump key was pressed
        jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    }
    
    // Handles idle timer and long idle animation
    private void Idle()
    {
        // Check if the player provides any movement or jump input
        bool hasInput = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // If any input is detected, reset idle timer and disable long idle animation
        if (hasInput)
        {
            idleTimer = 0f;
            animator.SetBool("isIdleTooLong", false);
            return;
        }

        // Increase idle timer while no input is given
        idleTimer += Time.deltaTime;

        // Activate long idle animation after defined idle time
        if (idleTimer >= longIdleTime)
            animator.SetBool("isIdleTooLong", true);
    }
        
    // Handles horizontal movement, running animation and sprite flipping
    private void Run()
    {
        // Apply horizontal movement
        playerRigidbody.linearVelocityX = horizontalInput * moveSpeed;

        // Set running animation state based on movement input
        animator.SetBool("isRunning", Mathf.Abs(horizontalInput) > 0.01f);

        // Flip sprite depending on movement direction
        if (horizontalInput != 0f)
            spriteRenderer.flipX = horizontalInput < 0f;
    }
      
    // Handles jumping logic and jump animation
    private void Jump()
    {
        // Jump only if jump key is pressed and player is on the ground
        if (jumpPressed && grounded)
        {   
            // Apply upward jump velocity
            playerRigidbody.linearVelocityY = jumpSpeed;

            // Set jumping animation state
            animator.SetBool("isJumping", true);

            // Play jump sound effect via PlayerAudio (if available)
            GetComponent<PlayerAudio>()?.PlayJump();
        }

        // Reset jumping animation when player lands (or is falling down onto ground)
        if (grounded && playerRigidbody.linearVelocityY <= 0f)
            animator.SetBool("isJumping", false);
        
    }

    // Method that checks if the player is touching the ground
    public bool IsGrounded()
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

    // Method that handles player death (animation + physics + game over)
    public void Die()
    {   
        // Stop if the player already died
        if (isDead) return;
        isDead = true;

        // Play death sound effect via PlayerAudio (if available)
        GetComponent<PlayerAudio>()?.PlayDeath();

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
        GameManager.Instance.ShowGameOver();
    }

    // Disables player movement and input
    public void DisableMovement()
    {
        // Disable movement input
        movementEnabled = false;

        // Stop any current movement immediately
        playerRigidbody.linearVelocity = Vector2.zero;

        // Stop running and jumping animations
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
    }
}
