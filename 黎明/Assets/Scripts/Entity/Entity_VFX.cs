using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("受击效果")]
    [SerializeField] private Material onDamageVFX;
    [SerializeField] private float onDamageVFXDuration = .2f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;

    [Header("攻击动画")]
    [SerializeField] private Color color;
    [SerializeField] private GameObject hitVFX;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }

    /// <summary>
    /// 播放受击效果
    /// </summary>
    public void PlayOnDamageVFX()
    {
        if(onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);

        onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCo());
    }

    private IEnumerator OnDamageVFXCo()
    {
        sr.material = onDamageVFX;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }

    /// <summary>
    /// 创建攻击特效
    /// </summary>
    /// <param name="target">目标位置</param>
    public void CreateHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = color;
    }
}
