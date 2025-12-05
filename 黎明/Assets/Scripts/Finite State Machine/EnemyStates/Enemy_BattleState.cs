using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastBattleTime;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player == null)
            player = enemy.DetectPlayer().transform;

        if(ShouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        //如果能检测到玩家，则不断更新战斗时间
        if(enemy.DetectPlayer())
            UpdateBattleTime();

        //如果战斗状态解除了，则转为待机状态
        if(BattleStateIsOver())
            stateMachine.ChangeState(enemy.idleState);

        //如果在攻击范围内并且检测到了玩家，则切换到攻击状态
        if(WithinAttackRange() && enemy.DetectPlayer())
            stateMachine.ChangeState(enemy.attackState);
        else//否则向玩家方向追击
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
    }

    
    private void UpdateBattleTime() => lastBattleTime = Time.time;

    private bool BattleStateIsOver() => Time.time > lastBattleTime + enemy.battleTimeDuration;

    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() <= 1;

    private float DistanceToPlayer()
    {
        if(player == null)
            return float.MaxValue;
        
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    /// <summary>
    /// 玩家相对于敌人的方向
    /// </summary>
    /// <returns></returns>
    private int DirectionToPlayer()
    {
        if(player == null)
            return 0;
        
        return player.position.x < enemy.transform.position.x ? -1 : 1;
    }
}
