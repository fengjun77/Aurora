using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour,IDamagable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    private Slider healthBar;
    private Entity_Stats stats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("生命恢复")]
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegenrateHealth = true;

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
        currentHealth = stats.GetMaxHealth();
        UpdateHealthBarUI();

        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    /// <summary>
    /// 受到伤害逻辑
    /// </summary>
    /// <param name="damage">受到的伤害</param>
    /// <param name="targetDealer">伤害输出者</param>
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
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
        float physicalDamageTaken = damage * (1 - mitigation);
        //处理根据元素抗性的减伤逻辑
        float resistance = stats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        //执行受击击退逻辑
        Vector2 knockback = CalculateKnockback(physicalDamageTaken, damageDealer);
        float duration = CalculateDuration(physicalDamageTaken);
        entity.ReciveKnockback(knockback, duration);

        ReduceHealth(physicalDamageTaken + elementalDamageTaken);
        Debug.Log(element + "元素伤害造成了" + elementalDamageTaken + "点血量");

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

    /// <summary>
    /// 自动回血逻辑
    /// </summary>
    private void RegenerateHealth()
    {
        if(!canRegenrateHealth)
            return;

        float regenAmount = stats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    /// <summary>
    /// 回血逻辑
    /// </summary>
    /// <param name="healthAmount"></param>
    public void IncreaseHealth(float healthAmount)
    {
        if(isDead)
           return;

        float newHealth = currentHealth + healthAmount;
        float maxHealth = stats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBarUI();
    }

    /// <summary>
    /// 扣血逻辑
    /// </summary>
    /// <param name="damage"></param>
    public void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFX();
        currentHealth -= damage;
        UpdateHealthBarUI();
        if(currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBarUI() => healthBar.value = currentHealth/stats.GetMaxHealth();

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
