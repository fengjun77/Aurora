using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum ElementType
{
    None,
    Fire,
    Ice,
    Lightning
}

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHp;
    //主属性
    public Stat_Major major;
    //进攻属性
    public Stat_Offense offense;
    //防御属性
    public Stat_Defense defense;


    /// <summary>
    /// 获取当前血量上限
    /// </summary>
    /// <returns></returns>
    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;

        return baseHp + bonusHp;
    }

    /// <summary>
    /// 获取物理伤害
    /// </summary>
    /// <param name="isCrit">是否暴击</param>
    /// <returns></returns>
    public float GetPhysicalDamage(out bool isCrit, float scale = 1)
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue() * 8; //每点强壮增加8伤害
        //总基础攻击力
        float totalBaseDamage = baseDamage + bonusDamage;

        //计算暴击率
        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * 2f; //每点敏捷增加2点暴击率

        float totalCritChance = baseCritChance + bonusCritChance;

        //计算暴击伤害
        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.intelligence.GetValue() * 6f; //每点智慧增加6点暴击伤害

        float totalCritPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = Random.Range(0,100) < totalCritChance;
        
        float finalDamage = isCrit ? totalBaseDamage * (1 + totalCritPower) : totalBaseDamage;

        return finalDamage * scale; 
    }

    /// <summary>
    /// 获取元素伤害
    /// </summary>
    /// <returns></returns>
    public float GetElementalDamage(out ElementType type, float scale)
    {
        float fireDamage = offense.fireDamage.GetValue();
        float iceDamage = offense.iceDamage.GetValue();
        float lightningDamage = offense.lightningDamage.GetValue();

        float bonusElementalDamage = major.intelligence.GetValue(); //每点智慧增加一点魔法伤害

        float highestDamage = fireDamage;
        type = ElementType.Fire;

        if(iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            type = ElementType.Ice;
        }

        if(lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            type = ElementType.Lightning;
        }

        if(highestDamage <= 0)
        {
            type = ElementType.None;
            return 0;
        }

        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage/2;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage/2;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage/2;

        float finalElementalDamage = highestDamage + bonusElementalDamage + bonusFire + bonusIce + bonusLightning;

        return finalElementalDamage * scale;
    }

    /// <summary>
    /// 获得护甲值
    /// </summary>
    /// <returns></returns>
    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue(); //每点活力增加一点护甲值
        float totalArmor = baseArmor + bonusArmor;

        float reductionMutliplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMutliplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    /// <summary>
    /// 获得护甲穿透值
    /// </summary>
    /// <returns></returns>
    public float GetArmorReduction()
    {
        //护甲穿透比例
        float finalReduction = offense.armorReduction.GetValue() / 100;

        return finalReduction;
    }

    /// <summary>
    /// 获取元素抗性
    /// </summary>
    /// <returns></returns>
    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue() * .5f;

        switch(element)
        {
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue();
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightingRes.GetValue();
                break;    
        }

        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 70;
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap);
    
        return finalResistance/100;
    }

    /// <summary>
    /// 获取闪避率
    /// </summary>
    /// <returns></returns>
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f; //每点敏捷增加 0.5%的闪避率

        float totalEvation = baseEvasion + bonusEvasion;
        float evasionCap = 40f;
        
        //让闪避率保持在特定范围内
        float finalEvasion = Mathf.Clamp(totalEvation, 0, evasionCap); 

        return finalEvasion;
    }
}
