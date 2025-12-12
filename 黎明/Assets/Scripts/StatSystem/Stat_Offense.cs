using System;
using UnityEngine;

[Serializable]
public class Stat_Offense
{
    //物理伤害
    public Stat damage; //基础伤害
    public Stat critPower; //暴击伤害
    public Stat critChance; //暴击率
    public Stat armorReduction; //护甲穿透

    //魔法伤害
    public Stat fireDamage; //火焰伤害
    public Stat iceDamage; //寒冰伤害
    public Stat lightningDamage; //雷电伤害
}
