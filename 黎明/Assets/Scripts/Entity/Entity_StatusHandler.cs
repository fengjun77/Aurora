using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_Stats stats;
    private Entity_VFX entityVfx;
    private Entity_Health health;
    private ElementType currentEffect = ElementType.None;

    [Header("导电效果相关")]
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maxCharge = 1;
    private Coroutine electrifyCoroutine;

    void Awake()
    {
        entity = GetComponent<Entity>();
        stats = GetComponent<Entity_Stats>();
        entityVfx = GetComponent<Entity_VFX>();
        health = GetComponent<Entity_Health>();
    }

    /// <summary>
    /// 实现冰冻效果
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="slowMultiplier">减速比例</param>
    public void ApplyChilledEffect(float duration,float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(ChilledEffectCoroutine(reducedDuration, slowMultiplier));
    }

    /// <summary>
    /// 实现灼烧效果
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="damage"></param>
    public void ApplyBurnEffect(float duration, float damage)
    {
        float fireResistance = stats.GetElementalResistance(ElementType.Fire);
        float reducedDamage = damage * (1 - fireResistance);

        StartCoroutine(BurnEffectCoroutine(duration, reducedDamage));
    }

    public void ApplyElectrifyEffect(float duration, float damage, float charge)
    {
        float lightningResistance = stats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);

        currentCharge += finalCharge;
        if(currentCharge >= maxCharge)
        {
            Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
            health.ReduceHP(damage);
            StopElectrifyEffect();
            return;
        }

        if(electrifyCoroutine != null)
            StopCoroutine(electrifyCoroutine);

        electrifyCoroutine = StartCoroutine(ElectrifyEffectCoroutine(duration));
    }

    private void StopElectrifyEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVfx();
    }

    private IEnumerator ChilledEffectCoroutine(float duration, float slowMultiplier)
    {
        entity.SlowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, currentEffect);

        yield return new WaitForSeconds(duration);

        currentEffect = ElementType.None;
    }

    private IEnumerator BurnEffectCoroutine(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2; //每秒灼烧次数
        int tickCount = (int)(ticksPerSecond * duration);//总灼烧次数

        float damagePerTick = totalDamage / tickCount; //每秒灼烧伤害
        float tickInterval = 1f / ticksPerSecond; //灼烧间隔时间

        for(int i = 0; i < tickCount; i++)
        {
            health.ReduceHP(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }

        currentEffect = ElementType.None;
    }

    private IEnumerator ElectrifyEffectCoroutine(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, currentEffect);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }

    /// <summary>
    /// 实现元素效果 确保在实现元素效果时，对象身上没有元素
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public bool CanBeApplied(ElementType element)
    {
        if(element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }
}
