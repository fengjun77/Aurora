using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string stateName;

    //初始化状态
    public EntityState(Player player, StateMachine stateMachine, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    //进入状态
    public virtual void Enter()
    {
        Debug.Log("我进入了" + stateName);
    }
    //状态中
    public virtual void Update()
    {
        Debug.Log("我正在" + stateName);
    }
    //退出状态
    public virtual void Exit()
    {
        Debug.Log("我退出了" + stateName);
    }
}
