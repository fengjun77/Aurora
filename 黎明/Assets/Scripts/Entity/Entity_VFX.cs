using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] private Color defaultColor;
    [SerializeField] private GameObject hitVFX;

    [Header("元素受击")]
    [SerializeField] private Color chillVfx = Color.cyan;
    [SerializeField] private Color burnVfx = Color.red;
    [SerializeField] private Color electrifyVfx = Color.yellow;
    private Color originalHitVfxColor;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = defaultColor;
    }

    /// <summary>
    /// 播放受击效果
    /// </summary>
    public void PlayOnDamageVFX()
    {
        if(onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);

        onDamageVFXCoroutine = StartCoroutine(OnDamageVFXCoroutine());
    }

    private IEnumerator OnDamageVFXCoroutine()
    {
        sr.material = onDamageVFX;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }

    /// <summary>
    /// 根据元素改变攻击特效颜色
    /// </summary>
    /// <param name="element"></param>
    public void UpdateOnHitColor(ElementType element)
    {
        if(element == ElementType.Ice)
            defaultColor = chillVfx;
        if(element == ElementType.Fire)
            defaultColor = burnVfx;
        if(element == ElementType.Lightning)
            defaultColor = electrifyVfx;

        if(element == ElementType.None)
            defaultColor = originalHitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if(element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCoroutine(duration, chillVfx));
            
        if(element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCoroutine(duration, burnVfx));

        if(element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCoroutine(duration, electrifyVfx));
        
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    /// <summary>
    /// 元素受击特效显示协程
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="effectColor"></param>
    /// <returns></returns>
    private IEnumerator PlayStatusVfxCoroutine(float duration, Color effectColor)
    {
        float tickInterval = .25f;
        float timer = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .9f;

        bool toggle = false;

        while(timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }

        sr.color = Color.white;
    }

    /// <summary>
    /// 创建攻击特效
    /// </summary>
    /// <param name="target">目标位置</param>
    public void CreateHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = defaultColor;
    }
}
