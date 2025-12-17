using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpSpeed = 12f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        this.playerRigidbody.linearVelocityX = horizontalInput * this.moveSpeed;

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        if (jumpPressed && this.IsGrounded())
        {
            this.playerRigidbody.linearVelocityY = this.jumpSpeed;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(this.groundCheck.position, this.groundCheckRadius, this.groundLayer);
    }
}
