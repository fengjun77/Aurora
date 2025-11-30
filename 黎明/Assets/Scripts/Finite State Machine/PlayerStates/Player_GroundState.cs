using Unity.VisualScripting;
using UnityEngine;

public class Player_GroundState : EntityState
{
    public Player_GroundState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if(rb.linearVelocity.y < 0 && !player.isGround)
        {
            stateMachine.ChangeState(player.fallState);
        }
        
        if(input.Player.Jump.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
