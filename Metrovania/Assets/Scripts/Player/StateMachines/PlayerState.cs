using UnityEngine;

public abstract class PlayerState
{
    protected Player player;

    protected Animator animator;

    protected Rigidbody2D rb;

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

    public PlayerState(Player player)
    {
        this.player = player;
        this.animator = player.animator;
        this.rb = player.rb;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void FixedUpdate() { }

    public virtual void Exit() { }
}
