using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    //初始化状态
    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    //进入状态
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        //Debug.Log("我进入了" + animBoolName);
    }
    //状态中
    public virtual void Update()
    {
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        //Debug.Log("我正在" + animBoolName);
    }
    //退出状态
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
        Debug.Log("我退出了" + animBoolName);
    }
}
