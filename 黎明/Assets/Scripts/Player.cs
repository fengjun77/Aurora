using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet input{ get; private set; }

    //玩家死亡事件
    public static event Action OnPlayerDeath;

    #region 角色状态
    public Player_IdleState idleState{ get; private set; }
    public Player_MoveState moveState{ get; private set; }
    public Player_JumpState jumpState{ get; private set; }
    public Player_FallState fallState{ get; private set; }
    public Player_WallSlideState wallSlideState{ get; private set; }
    public Player_WallJumpState wallJumpState{ get; private set; }
    public Player_DashState dashState{ get; private set; }
    public Player_BasicAttackState basicAttackState{ get; private set; }
    public Player_JumpAttackState jumpAttackState{ get; private set; }
    public Player_DeadState deadState {get; private set; }
    #endregion

    [Header("基础参数")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpForce;
    //允许跳跃攻击的最大距离
    public float distanceCanJumpAttack;
    [Range(0,1)]
    public float inAirMoveMultiplier;
    [Range(0,1)]
    public float wallSlideMultiplier;

    [Header("冲刺参数")]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20f;
    [Header("攻击参数")]
    //攻击时的移动速度
    public Vector2[] attackVelocity;
    //跳跃攻击参数
    public Vector2 jumpAttackVelocity;
    //攻击时速度持续时间
    public float attackVelocityDuration;
    public float comboResetTime = 1f;
    private Coroutine attackCoroutine;
    
    //射线检测起点的Y轴偏移(玩家距离自己脚底的距离)
    public float rayOffsetY;
    //距离地面的射线长度(用于检测是否允许跳跃攻击,需要设计的稍微大一点)
    [SerializeField]
    private float groundDistance;

    public bool canJumpAttack{ get; private set; }
    
    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this,stateMachine,"idle");
        moveState = new Player_MoveState(this,stateMachine,"move");
        jumpState = new Player_JumpState(this,stateMachine,"jumpFall");
        fallState = new Player_FallState(this,stateMachine,"jumpFall");
        wallSlideState = new Player_WallSlideState(this,stateMachine,"wallSlide");
        wallJumpState = new Player_WallJumpState(this,stateMachine,"jumpFall");
        dashState = new Player_DashState(this,stateMachine,"dash");
        basicAttackState = new Player_BasicAttackState(this,stateMachine,"basicAttack");
        jumpAttackState = new Player_JumpAttackState(this,stateMachine,"jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;


    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();

        canJumpAttack = HandleGroundDistanceCheck();
    }

    void OnDisable()
    {
        input.Disable();
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        //调用所有订阅了的函数
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    /// <summary>
    /// 检测玩家与地面的距离，决定是否允许跳跃攻击
    /// </summary>
    /// <returns></returns>
    private bool HandleGroundDistanceCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, rayOffsetY, 0), Vector2.down, groundDistance, groundLayer);
        
        if(hit.distance > distanceCanJumpAttack)
            return true;

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, rayOffsetY, 0), transform.position + Vector3.down * groundDistance);
    }

    public void StartAttackCoroutine()
    {
        if(attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(ChangeAttackWithDelay());
    }

    private IEnumerator ChangeAttackWithDelay()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}
