using UnityEngine;

public class SlideState : PlayerState
{
    private float slideTimer;
    private float slideStopTimer;

    public SlideState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();

        slideTimer = player.slideDuration;
        slideStopTimer = 0;
        player.SetColliderSlide();
        animator.SetBool("isSliding", true);
    }

    public override void Update()
    {
        base.Update();

        if (slideTimer > 0)
        {
            slideTimer -= Time.deltaTime;
        }
        else if (slideStopTimer <= 0)
        {
            slideStopTimer = player.slideStopDuration;
        }
        else
        {
            slideStopTimer -= Time.deltaTime;

            if(slideStopTimer <= 0)
            {
                if(player.CheckCieling() || MoveInput.y < -0.1f)
                {
                    player.ChangeState(player.crouchState);
                }
                else
                {
                    player.ChangeState(player.idleState);
                }
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(slideTimer > 0)
        {
            rb.linearVelocity = new Vector2(player.slideSpeed * player.facingDirection, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderNormal();
        animator.SetBool("isSliding", false);
    }
    
}
