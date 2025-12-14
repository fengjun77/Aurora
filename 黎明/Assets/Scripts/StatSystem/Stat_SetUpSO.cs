using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Stat Set Up", fileName = "Default Stat")]
public class Stat_SetUpSO : ScriptableObject
{
    [Header("基础属性")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("主要属性")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("进攻属性 - 物理")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 20;
    public float armorReduction;

    [Header("进攻属性 - 元素")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("防御属性 - 物理")]
    public float armor;
    public float evasion;

    [Header("防御属性 - 元素")]
    public float fireRes;
    public float iceRes;
    public float lightningRes;
}
