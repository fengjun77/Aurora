using Unity.VisualScripting;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        HandleWallSlide();

        if(input.Player.Jump.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }


        if(!player.isWall)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if(player.isGround)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void HandleWallSlide()
    {
        if(player.moveInput.y < 0)
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        else
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideMultiplier);
        
    }
}
