using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Animator anim;
    private Entity_VFX vfx;
    private Rigidbody2D rb;

    [SerializeField] private Vector2 knockback;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vfx = GetComponent<Entity_VFX>();
    }

    public void TakeDamage(float damage, Transform damageDealer)
    {
        vfx.PlayOnDamageVFX();
        
        anim.SetBool("openChest",true);

        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-20,20);

        rb.simulated = false;

        //TODO:物品掉落逻辑
    }
}
