using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform targetDealer)
    {
        bool wasHit = base.TakeDamage(damage, elementalDamage, element, targetDealer);

        if(!wasHit)
            return false;
        
        //如果被玩家攻击了，则进入追击状态，将目标设为玩家
        if(targetDealer.GetComponent<Player>())
            enemy.TryChangeBattleState(targetDealer);

        return true;

    }
}
