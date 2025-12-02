using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }
}
