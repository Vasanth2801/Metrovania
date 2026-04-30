using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        animator.SetBool("isJumping", true);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);

        JumpPressed = false;
        JumpReleased = false;
    }

    public override void Update()
    {
        base.Update();

        if(player.isGrounded && rb.linearVelocity.y <= 0)
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        player.ApplyGravity();

        if(JumpReleased && rb.linearVelocity.y > 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * player.jumpMultiplier);
            JumpReleased = false;
        }

        float speed = RunPressed ? player.runSpeed : player.walkSpeed;
        rb.linearVelocity = new Vector2(player.facingDirection * speed, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("isJumping", false);
    }
}