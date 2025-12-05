using UnityEngine;

public class Enemy_IdleState : Enemy_GroundState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        startTime = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if(startTime < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
