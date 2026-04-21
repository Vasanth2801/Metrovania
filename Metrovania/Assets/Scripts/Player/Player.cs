using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public int facingDirection = 1;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    [Header("Inputs")]
    public Vector2 moveInput;
    [SerializeField] private bool runPressed;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpMultiplier = 0.5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;

    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float slideSpeed = 12f;
    [SerializeField] private bool isSliding = false;
    private float slideHeight;
    private float slideTimer;
    public float normalHeight;
    public Vector2 normalOffSet;
    public Vector2 slideOffSet;

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
        HandleAnimations();
        HandleSlide();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnRun(InputValue value)
    {
        runPressed = value.isPressed;
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

    void HandleSlide()
    {
        if(isSliding)
        {
            slideTimer -= Time.deltaTime;

            if(slideTimer < 0)
            {
                isSliding = false;
            }


            if(isGrounded && runPressed && moveInput.y < -0.1f && isGrounded)
            {
                isSliding = true;
                slideTimer = slideDuration;
                rb.linearVelocity = new Vector2(slideSpeed * facingDirection,rb.linearVelocity.y);
            }
        }
    }

    void HandleMovement()
    {
        float currentSpeed = runPressed ? runSpeed : walkSpeed;
        float targetSpeed = moveInput.x * currentSpeed;
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

    void HandleAnimations()
    {

        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f && isGrounded;

        animator.SetBool("isIdling", !isMoving && isGrounded);

        animator.SetBool("isWalking", isMoving && !runPressed);

        animator.SetBool("isRunning", isMoving && runPressed);

        animator.SetBool("isJumping", rb.linearVelocity.y > 0.1f);

        animator.SetBool("isGrounded", isGrounded);

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
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