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

    #endregion

    [Header("基础参数")]
    public float moveSpeed;
    public float jumpForce;
    [Range(0,1)]
    public float inAirMoveMultiplier;
    [Range(0,1)]
    public float wallSlideMultiplier;
    private bool facingRight = true;
    private int facingDir = 1;

    [Header("碰撞检测")]
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private float wallCheckDistance;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isGround{ get; private set; }
    public bool isWall{ get; private set; }
    

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
        HandleGroundCheck();
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

    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    private void HandleGroundCheck()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isWall = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * (facingDir * wallCheckDistance));
    }

    void OnDisable()
    {
        input.Disable();
    }
}
