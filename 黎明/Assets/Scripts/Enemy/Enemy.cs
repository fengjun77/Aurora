using System.Data.Common;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;


    [Header("移动参数")]
    public float idleTime;
    public float moveSpeed;
    [Range(0,2)]
    public float moveAnimSpeedMultiplier;

    [Header("战斗参数")]
    public float battleMoveSpeed = 2.8f;
    public float attackDistance = 2;
    //在失去目标后，在战斗状态中持续的时间
    public float battleTimeDuration = 5;
    public float retreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("眩晕参数")]
    public float stunnedDuration = 2f;
    public Vector2 stunnedVelocity = new Vector2(7,7);
    [SerializeField] protected bool canStunned;

    [Header("检测玩家参数")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player {get; private set;}

    protected override void Start()
    {
        base.Start();

        stateMachine.Init(idleState);
    }

    void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    /// <summary>
    /// 玩家死亡时怪物需要处理的逻辑
    /// </summary>
    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void EnableCounterWindow(bool enable) => canStunned = enable;

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
    }

    public void TryChangeBattleState(Transform player)
    {
        this.player = player;

        if(stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;

        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerTransform()
    {
        if(player == null)
            player = DetectPlayer().transform;
        
        return player;
    }

    /// <summary>
    /// 检测玩家
    /// </summary>
    /// <returns>玩家信息</returns>
    public RaycastHit2D DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position,Vector2.right * facingDir, playerCheckDistance, playerLayer | groundLayer);
    
        if(hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position,new Vector3(playerCheck.position.x + facingDir * playerCheckDistance,playerCheck.position.y));
    }
}
