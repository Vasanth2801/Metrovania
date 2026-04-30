using UnityEngine;

public class AttackState : PlayerState
{
   public AttackState(Player player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("isAttacking", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void AttackAnimationFinished()
    {
        base.AttackAnimationFinished();

        if(Mathf.Abs(MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        }
        else
        {
            player.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("isAttacking", false);
    }
}