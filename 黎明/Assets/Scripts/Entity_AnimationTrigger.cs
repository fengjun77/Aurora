using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat combat;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
        combat = GetComponentInParent<Entity_Combat>();
    }

    public void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }

    public void AttackTrigger()
    {
        combat.PerformAttack();
    }
}
