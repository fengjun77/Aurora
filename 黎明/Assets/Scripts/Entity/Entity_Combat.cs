using UnityEngine;

//战斗相关
public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    public float damage;

    //检测中心点位置
    [SerializeField] private Transform targetCheck;
    //检测范围半径
    [SerializeField] private float targetCheckRadius;
    //攻击目标的层
    [SerializeField] private LayerMask targetLayer;

    void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
    }

    /// <summary>
    /// 执行攻击逻辑
    /// </summary>
    public void PerformAttack()
    {
        //遍历所有被攻击的人，将自己的伤害和自己的位置信息传出
        foreach(var target in GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if(damagable == null)
                continue;

            damagable.TakeDamage(damage,transform);
            entityVFX.CreateHitVFX(target.transform);
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
