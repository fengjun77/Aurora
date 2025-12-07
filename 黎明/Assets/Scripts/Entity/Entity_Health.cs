using UnityEngine;

public class Entity_Health : MonoBehaviour,IDamagable
{
    private Entity_VFX entityVFX;
    private Entity entity;

    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("受击击退")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(2.5f, 3);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .7f;

    [Header("重击")]
    [SerializeField] private float heavyDamageThreshold = .3f;

    void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
    }

    void Start()
    {
        currentHp = maxHP;
    }

    /// <summary>
    /// 受到伤害逻辑
    /// </summary>
    /// <param name="damage">受到的伤害</param>
    /// <param name="targetDealer">伤害输出者</param>
    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        entityVFX?.PlayOnDamageVFX();

        //执行受击击退逻辑
        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);
        entity.ReciveKnockback(knockback, duration);
        ReduceHP(damage);
    }

    //扣血逻辑
    protected void ReduceHP(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    /// <summary>
    /// 计算受击方向
    /// </summary>
    /// <param name="damagerDealer">伤害输出者</param>
    /// <returns></returns>
    private Vector2 CalculateKnockback(float damage, Transform damagerDealer)
    {
        int dir = transform.position.x > damagerDealer.position.x ? 1 : -1;

        Vector2 knockback = isHeavyAttack(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x *= dir;

        return knockback;  
    }

    private float CalculateDuration(float damage) => isHeavyAttack(damage) ? heavyKnockbackDuration : knockbackDuration;

    //如果大于阈值，则该攻击为重击
    private bool isHeavyAttack(float damage) => damage/maxHP > heavyDamageThreshold;
}
