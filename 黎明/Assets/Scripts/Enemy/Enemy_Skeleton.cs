using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    public bool CanbeCounter {get => canStunned; }

    public void HandleCounter()
    {
        if(CanbeCounter == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
}
