using System.Net;
using UnityEngine;

//战斗相关
public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stats;
    public float damage;

    //检测中心点位置
    [SerializeField] private Transform targetCheck;
    //检测范围半径
    [SerializeField] private float targetCheckRadius;
    //攻击目标的层
    [SerializeField] private LayerMask targetLayer;

    [Header("元素状态属性")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillSLowMultiplier = .4f;
    [SerializeField] private float electrifyChargeBuildUp = .4f;

    [Space]
    [SerializeField] private float fireScale = .6f;
    [SerializeField] private float lightningScale = 2.5f;

    void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    /// <summary>
    /// 执行攻击逻辑
    /// </summary>
    public void PerformAttack()
    {
        //遍历所有被攻击的人，将自己的伤害和自己的位置信息传出
        foreach(var target in GetDetectedColliders())
        {
            //获得敌人身上的受击代码
            IDamagable damagable = target.GetComponent<IDamagable>();
            if(damagable == null)
                continue;

            float damage = stats.GetPhysicalDamage(out bool isCrit);
            float elementalDamage = stats.GetElementalDamage(out ElementType element, .6f);
            bool targetGotHit = damagable.TakeDamage(damage, elementalDamage, element, transform);
                
            if(element != ElementType.None)
                ApplyStatusEffect(target.transform, element);

            //如果造成伤害
            if(targetGotHit)
            {
                //根据元素改变受击颜色
                entityVFX.UpdateOnHitColor(element);
                entityVFX.CreateHitVFX(target.transform);
            }
        }
    }

    /// <summary>
    /// 执行元素效果
    /// </summary>
    /// <param name="target"></param>
    /// <param name="element"></param>
    public void ApplyStatusEffect(Transform target, ElementType element, float scale = 1)
    {
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
        if(statusHandler == null)
            return;

        if(element == ElementType.Ice && statusHandler.CanBeApplied(ElementType.Ice))
            statusHandler.ApplyChilledEffect(defaultDuration, chillSLowMultiplier);

        if(element == ElementType.Fire && statusHandler.CanBeApplied(ElementType.Fire))
        {
            scale = fireScale;
            float fireDamage = stats.offense.fireDamage.GetValue() * scale;
            statusHandler.ApplyBurnEffect(defaultDuration, fireDamage);
        }

        if(element == ElementType.Lightning && statusHandler.CanBeApplied(ElementType.Lightning))
        {
            scale = lightningScale;
            float lightningDamage = stats.offense.lightningDamage.GetValue() * scale;
            statusHandler.ApplyElectrifyEffect(defaultDuration, lightningDamage, electrifyChargeBuildUp);
        }
    }

    //获得攻击范围内的所有敌人
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position,targetCheckRadius,targetLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position,targetCheckRadius);
    }
}
