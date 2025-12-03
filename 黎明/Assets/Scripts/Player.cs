using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim{ get; private set; }
    public Rigidbody2D rb{ get; private set; }

    public PlayerInputSet input{ get; private set; }

    private StateMachine stateMachine;
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
    #endregion

    [Header("基础参数")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpForce;
    //允许跳跃攻击的最大距离
    public float distanceCanJumpAttack;

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

    [Range(0,1)]
    public float inAirMoveMultiplier;
    [Range(0,1)]
    public float wallSlideMultiplier;
    private bool facingRight = true;
    public int facingDir{ get; private set; } = 1;

    [Header("碰撞检测")]
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private float wallCheckDistance;
    [SerializeField]
    private Transform handWallCheck;
    [SerializeField]
    private Transform footWallCheck;
    //射线检测起点的Y轴偏移(玩家距离自己脚底的距离)
    public float rayOffsetY;
    //距离地面的射线长度(用于检测是否允许跳跃攻击,需要设计的稍微大一点)
    [SerializeField]
    private float groundDistance;

    [SerializeField]
    private LayerMask groundLayer;
    public bool isGround{ get; private set; }
    public bool isWall{ get; private set; }
    public bool canJumpAttack{ get; private set; }
    

    public Vector2 moveInput { get; private set; }  

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInputSet();
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this,stateMachine,"idle");
        moveState = new Player_MoveState(this,stateMachine,"move");
        jumpState = new Player_JumpState(this,stateMachine,"jumpFall");
        fallState = new Player_FallState(this,stateMachine,"jumpFall");
        wallSlideState = new Player_WallSlideState(this,stateMachine,"wallSlide");
        wallJumpState = new Player_WallJumpState(this,stateMachine,"jumpFall");
        dashState = new Player_DashState(this,stateMachine,"dash");
        basicAttackState = new Player_BasicAttackState(this,stateMachine,"basicAttack");
        jumpAttackState = new Player_JumpAttackState(this,stateMachine,"jumpAttack");
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void Start()
    {
        stateMachine.Init(idleState);
    }

    void Update()
    {
        HandleColCheck();
        canJumpAttack = HandleGroundDistanceCheck();
        stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float x,float y)
    {
        rb.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    private void HandleFlip(float x)
    {
        if(x > 0 && !facingRight)
        {
            Flip();
        }
        else if(x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    /// <summary>
    /// 检测玩家是否接触地面和墙壁
    /// </summary>
    private void HandleColCheck()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isWall = Physics2D.Raycast(handWallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer)
               && Physics2D.Raycast(footWallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(handWallCheck.position, handWallCheck.position + Vector3.right * (facingDir * wallCheckDistance));
        Gizmos.DrawLine(footWallCheck.position, footWallCheck.position + Vector3.right * (facingDir * wallCheckDistance));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, rayOffsetY, 0), transform.position + Vector3.down * groundDistance);
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
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

    void OnDisable()
    {
        input.Disable();
    }
}
