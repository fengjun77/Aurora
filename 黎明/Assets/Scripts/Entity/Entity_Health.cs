using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamagable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats stats;

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
        stats =GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
    }

    void Start()
    {
        currentHp = stats.GetMaxHealth();
        UpdateHealthBarUI();
    }

    /// <summary>
    /// 受到伤害逻辑
    /// </summary>
    /// <param name="damage">受到的伤害</param>
    /// <param name="targetDealer">伤害输出者</param>
    public virtual bool TakeDamage(float damage, float elementalDamage, Transform damageDealer)
    {
        if(isDead)
            return false;
        
        //判断是否可以闪避伤害
        if(DodgeAttack())
            return false;
        
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        //处理根据护甲值的减伤逻辑
        float mitigation = stats.GetArmorMitigation(armorReduction);
        float finalDamage = damage * (1 - mitigation);

        entityVFX?.PlayOnDamageVFX();

        //执行受击击退逻辑
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);
        entity.ReciveKnockback(knockback, duration);
        ReduceHP(finalDamage + elementalDamage);
        Debug.Log("元素伤害造成了" + elementalDamage + "点血量");
        return true;
    }


    /// <summary>
    /// 闪避攻击
    /// </summary>
    /// <returns></returns>
    private bool DodgeAttack()
    {
        return Random.Range(0,100) < stats.GetEvasion();
    }

    //扣血逻辑
    protected void ReduceHP(float damage)
    {
        currentHp -= damage;
        UpdateHealthBarUI();
        if(currentHp <= 0)
            Die();
    }

    private void UpdateHealthBarUI() => healthBar.value = currentHp/stats.GetMaxHealth();

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
    private bool isHeavyAttack(float damage) => damage/stats.GetMaxHealth() > heavyDamageThreshold;
}
