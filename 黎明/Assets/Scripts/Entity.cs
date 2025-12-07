using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim{ get; private set; }
    public Rigidbody2D rb{ get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDir{ get; private set; } = 1;

    [Header("碰撞检测")]
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private float wallCheckDistance;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform handWallCheck;
    [SerializeField]
    private Transform footWallCheck;
    [SerializeField]
    protected LayerMask groundLayer;
    public bool isGround{ get; private set; }
    public bool isWall{ get; private set; }

    private bool isKnocked;
    private Coroutine knockbackCoroutine;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();       
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        HandleColCheck();
        
        stateMachine.UpdateActiveState();
    }

    public virtual void EntityDeath()
    {
        
    }

    public void SetVelocity(float x,float y)
    {
        //如果处于击退状态，则不设置速度
        if(isKnocked)
            return;

        rb.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    public void ReciveKnockback(Vector2 power, float duration)
    {
        if(knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(power,duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 power, float duration)
    {
        isKnocked = true;
        //施加向后退方向的力
        rb.linearVelocity = power;

        yield return new WaitForSeconds(duration);

        //让受力归零，让对象停住
        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void HandleFlip(float x)
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
    protected void HandleColCheck()
    {
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        if(footWallCheck != null)
            isWall = Physics2D.Raycast(handWallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer)
                  && Physics2D.Raycast(footWallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
        else
            isWall = Physics2D.Raycast(handWallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(handWallCheck.position, handWallCheck.position + Vector3.right * (facingDir * wallCheckDistance));
        if(footWallCheck != null)
            Gizmos.DrawLine(footWallCheck.position, footWallCheck.position + Vector3.right * (facingDir * wallCheckDistance));
        
    }
}
