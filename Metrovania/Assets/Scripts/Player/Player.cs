using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public int facingDirection = 1;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;

    [Header("Inputs")]
    public Vector2 moveInput;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpMultiplier = 0.5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    private void Start()
    {
        rb.gravityScale = normalGravity;
    }

    private void Update()
    {
        Flip();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if(value.isPressed)
        {
            jumpPressed = true;
            jumpReleased = false;
        }
        else
        {
            jumpReleased = true;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
        CheckGrounded();
        HandleJump();
    }

    void HandleMovement()
    {
        float targetSpeed = moveInput.x * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }

    void HandleJump()
    {
        if(jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
       
        if(jumpReleased)
        {
            if(rb.linearVelocity.y > 0.1f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpMultiplier);
            }
            jumpReleased = false;
        }
    }

    void ApplyGravity()
    {
        if(rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = fallGravity;
        }
        else if(rb.linearVelocity.y < 0.1f)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void Flip()
    {
        if (moveInput.x > 0.1f)
        {
            facingDirection = 1;
        }
        else if(moveInput.x < -0.1f)
        {
            facingDirection = -1;
        }

        transform.localScale = new Vector3(facingDirection, 1, 1);
    }
}
