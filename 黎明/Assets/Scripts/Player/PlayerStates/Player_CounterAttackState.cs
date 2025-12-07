using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat playerCombat;
    private bool counterSomebody;

    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        playerCombat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        startTime = playerCombat.GetCounterDuration();
        counterSomebody = playerCombat.CounterAttack();
        
        anim.SetBool("counterAttackPerformed", counterSomebody);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0,rb.linearVelocity.y);
        
        //当反击执行完成
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);

        //当超过反击时间并且没有可反击的敌人
        if(startTime < 0 && !counterSomebody)
            stateMachine.ChangeState(player.idleState);

    }
}
