using Unity.VisualScripting;
using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private float dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        startTime = player.dashDuration;
        dashDir = player.moveInput.x == 0 ? player.facingDir : (int)player.moveInput.x;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;     
    }

    public override void Update()
    {
        base.Update();

        CancelDash();

        player.SetVelocity(dashDir * player.dashSpeed, 0);

        if(startTime <= 0)
        {
            if(player.isGround)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0,0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDash()
    {
        if(player.isWall)
        {
            if(player.isGround)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
