using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTrigger
{
    private Enemy enemy;
    private Enemy_VFX enemyVfx;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<Enemy_VFX>();
    }

    public void OpenCounterWindow()
    {
        enemy.EnableCounterWindow(true);
        enemyVfx.EnableAttackWindow(true);
    }

    public void CloseCounterWindow()
    {
        enemy.EnableCounterWindow(false);
        enemyVfx.EnableAttackWindow(false);
    }
}
