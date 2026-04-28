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
    }
}