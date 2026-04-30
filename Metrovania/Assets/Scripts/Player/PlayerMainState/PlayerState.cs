using System.Runtime.InteropServices;
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;

    protected Animator animator;

    protected Rigidbody2D rb;

    protected Combat combat;

    protected bool JumpPressed
    {
        get => player.jumpPressed;
        set => player.jumpPressed = value;
    }

    protected bool JumpReleased
    {
        get => player.jumpReleased;
        set => player.jumpReleased = value;
    }

    protected bool RunPressed
    {
        get => player.runPressed;
        set => player.runPressed = value;
    }

    protected bool AttackPressed
    {
        get => player.attackPressed;
        set => player.attackPressed = value;
    }

    protected Vector2 MoveInput => player.moveInput;

    public PlayerState(Player player)
    {
        this.player = player;
        this.animator = player.animator;
        this.rb = player.rb;
        this.combat = player.combat;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void AttackAnimationFinished() { }

    public virtual void Exit() { }
}