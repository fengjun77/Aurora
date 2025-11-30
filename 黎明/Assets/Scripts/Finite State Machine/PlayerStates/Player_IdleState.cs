using UnityEngine;

public class Player_IdleState : Player_GroundState
{
    public Player_IdleState(Player player, StateMachine stateMachine,string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Additional logic for entering idle state
        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        // Additional logic for updating idle state
        if(player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // Additional logic for exiting idle state
    }
}
