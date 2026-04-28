using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(Player player) : base(player) { }


    public override void Enter()
    {
       animator.SetBool("isIdling", true);
       rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(JumpPressed)
        {
            player.ChangeState(player.jumpState);
        }
        else if(Mathf.Abs(MoveInput.x) > 0.1f)
        {
            player.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("isIdling", false);
    }
}