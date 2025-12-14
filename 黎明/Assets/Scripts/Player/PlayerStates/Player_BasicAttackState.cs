using System.Runtime.InteropServices;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private float lastAttackTime;
    //为了让玩家可以在连击中改变攻击方向
    private int attackDir;
    //是否继续攻击
    private bool continueAttack;


    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        continueAttack = false;
        //重置连击计时器和索引
        ResetComboIndex();

        //同步攻击速度
        SyncAttackSpeed();

        //如果玩家没有输入方向，则使用面朝方向作为攻击方向
        //否则更具输入的方向，改变玩家的面朝方向
        attackDir = player.moveInput.x == 0 ? player.facingDir : (int)player.moveInput.x;

        anim.SetInteger("comboIndex", comboIndex);

        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if(input.Player.Attack.WasPerformedThisFrame())
            SetComboAttack();

        //TODO: 添加检测和怪物伤害

        if(triggerCalled)
        {
            HandleStateExit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
        comboIndex++;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if(attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        attackVelocityTimer = player.attackVelocityDuration;
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        player.SetVelocity(attackDir * attackVelocity.x, attackVelocity.y);
    }

    private void ResetComboIndex()
    {
        if(comboIndex > comboLimit || Time.time > lastAttackTime + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }

    private void SetComboAttack()
    {
        //如果不是第三段攻击，则可以继续迅速衔接第一段攻击
        if(comboIndex < comboLimit)
            continueAttack = true;
    }

    private void HandleStateExit()
    {
        if(continueAttack)//如果继续攻击，切换到下一个攻击状态
        {
            anim.SetBool(animBoolName, false);
            player.StartAttackCoroutine();
        }
        else//攻击动画结束，返回待机状态
            stateMachine.ChangeState(player.idleState);
    }
}

