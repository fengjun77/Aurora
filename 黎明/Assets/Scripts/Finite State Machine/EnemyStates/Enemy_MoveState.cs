using UnityEngine;

public class Enemy_MoveState : Enemy_GroundState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(!enemy.isGround || enemy.isWall)
        {
            enemy.Flip();
        }
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        //如果没有检测到地面，或者检测到了墙壁，则变为待机并且转身
        if(!enemy.isGround || enemy.isWall)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
