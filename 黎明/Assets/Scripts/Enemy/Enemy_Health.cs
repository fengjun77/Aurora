using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform targetDealer)
    {
        base.TakeDamage(damage,targetDealer);

        if(isDead)
            return;
        
        //如果被玩家攻击了，则进入追击状态，将目标设为玩家
        if(targetDealer.GetComponent<Player>())
            enemy.TryChangeBattleState(targetDealer);

    }
}
