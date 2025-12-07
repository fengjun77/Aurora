using UnityEngine;

public class Player_MoveState : Player_GroundState
{
    public Player_MoveState(Player player, StateMachine stateMachine,string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Additional logic for entering move state
    }

    public override void Update()
    {
        base.Update();
        // Additional logic for updating move state
        if(player.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        // Additional logic for exiting move state
    }
}
