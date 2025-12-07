using UnityEngine;
using UnityEngine.VFX;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX enemyVfx;
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyVfx = enemy.GetComponent<Enemy_VFX>();
    }

    public override void Enter()
    {
        base.Enter();
        //关闭反击窗口图片
        enemyVfx.EnableAttackWindow(false);
        //解除可反击
        enemy.EnableCounterWindow(false);

        startTime = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(startTime < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
