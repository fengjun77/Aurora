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

    #endregion

    [Header("基础参数")]
    public float moveSpeed;
    public float jumpForce;

    private bool facingRight = true;

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
    }

    void OnDisable()
    {
        input.Disable();
    }
}
