using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stats;

    protected float startTime;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    //进入状态
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
        //Debug.Log("我进入了" + animBoolName);
    }
    //状态中
    public virtual void Update()
    {
        startTime -= Time.deltaTime;

        UpdateAnimationParameters();
        //Debug.Log("我正在" + animBoolName);
    }
    //退出状态
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
        //Debug.Log("我退出了" + animBoolName);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    /// <summary>
    /// 更新动画参数
    /// </summary>
    public virtual void UpdateAnimationParameters()
    {
        
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }
}
