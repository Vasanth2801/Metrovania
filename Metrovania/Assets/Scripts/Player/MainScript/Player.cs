using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region StateMachines
    [Header("Player Modular States")]
    public PlayerState currentState;

    public IdleState idleState;
    #endregion

    #region Variables
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public int facingDirection = 1;

    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] private CapsuleCollider2D playerCollider;

    [Header("Inputs")]
    public Vector2 moveInput;
    public bool runPressed;
    public bool jumpPressed;
    public bool jumpReleased;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpMultiplier = 0.5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;
 
    [Header("Slide Settings")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float slideSpeed = 12f;
    [SerializeField] private float slideStopDuration = 0.2f;
    [SerializeField] private bool isSliding = false;
    private float slideHeight;
    public float normalHeight;
    private float slideTimer;
    public Vector2 normalOffSet;
    public Vector2 slideOffSet;

    [Header("Crouch Settings")]
    [SerializeField] private Transform cielingCheck;
    [SerializeField] private float cielingRadius = 0.2f;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    #endregion

    #region Start and Update

    private void Awake()
    {
        idleState = new IdleState(this);
    
    }

    private void Start()
    {
        rb.gravityScale = normalGravity;

        ChangeState(idleState);
    }

    private void Update()
    {
        currentState.Update();

        Flip();
        HandleAnimations();
        HandleSlide();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();

        if (!isSliding)
        {
            HandleMovement();
        }
        ApplyGravity();
        CheckGrounded();
        HandleJump();
    }
    #endregion

    #region Input Handling
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
    #endregion

    #region Movement and Slide Methods
    public void ChangeState(PlayerState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
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


            if (isGrounded && runPressed && moveInput.y < -0.1f && !isSliding)
            {
                isSliding = true;
                slideTimer = slideDuration;
                rb.linearVelocity = new Vector2(slideSpeed * facingDirection, rb.linearVelocity.y);
            }
        }
    }
    #endregion

    #region Jump and Gravity Methods
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
    #endregion

    #region Sliding and Crouching Check Methods
    public bool CheckCieling()
    {
        return Physics2D.OverlapCircle(cielingCheck.position, cielingRadius, groundLayer);
    }

    public void SetColliderNormal()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, normalHeight);
        playerCollider.offset = normalOffSet;
    }

    public void SetColliderSlide()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, slideHeight);
        playerCollider.offset = slideOffSet;
    }
    #endregion

    #region Animations and Flip
    void HandleAnimations()
    {

        bool isMoving = Mathf.Abs(moveInput.x) > 0.1f && isGrounded;

        animator.SetBool("isIdling", !isMoving && isGrounded && !isSliding);

        animator.SetBool("isWalking", isMoving && !runPressed && !isSliding);

        animator.SetBool("isRunning", isMoving && runPressed && !isSliding);

        animator.SetBool("isJumping", rb.linearVelocity.y > 0.1f);

        animator.SetBool("isSliding", isSliding);

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
    #endregion
}