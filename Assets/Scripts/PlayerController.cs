using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player used for movement and jumping
    private Rigidbody2D playerRigidbody;

    // Horizontal movement speed
    [SerializeField] private float moveSpeed = 6f;
    // Jump force applied to the player
    [SerializeField] private float jumpSpeed = 12f;
    // Position used to check if the player is on the ground
    [SerializeField] private Transform groundCheck;
    // Radius of the ground check circle
    [SerializeField] private float groundCheckRadius = 0.15f;
    // Layer that represents the ground
    [SerializeField] private LayerMask groundLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Rigidbody2D component from the player object
        this.playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Read horizontal input (A/D or left/right arrows)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        // Apply horizontal movement
        this.playerRigidbody.linearVelocityX = horizontalInput * this.moveSpeed;

        // Check if any jump key was pressed
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        // Jump only if jump key is pressed and player is on the ground
        if (jumpPressed && this.IsGrounded())
        {
            this.playerRigidbody.linearVelocityY = this.jumpSpeed;
        }
    }


    // Method that checks if the player is touching the ground
    private bool IsGrounded()
    {
        // Overlap circle at groundCheck position to detect ground
        return Physics2D.OverlapCircle(this.groundCheck.position, this.groundCheckRadius, this.groundLayer);
    }
}
