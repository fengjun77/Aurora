using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
}
