using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;

    private StateMachine stateMachine;
    #region 角色状态
    public Player_IdleState idleState{ get; private set; }
    public Player_MoveState moveState{ get; private set; }

    #endregion

    public Vector2 moveInput { get; private set; }  

    void Awake()
    {
        input = new PlayerInputSet();
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this,stateMachine,"idle");
        moveState = new Player_MoveState(this,stateMachine,"move");
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

    void OnDisable()
    {
        input.Disable();
    }
}
