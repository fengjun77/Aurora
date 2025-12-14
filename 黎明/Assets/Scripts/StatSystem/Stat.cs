using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>(); //属性修正值列表

    public float GetValue()
    {
        return GetFinalValue();
    }

    //TODO:添加各种属性的计算

    /// <summary>
    /// 添加属性修正值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="source"></param>
    public void AddModifier(float value, string source)
    {
        StatModifier modToAdd = new StatModifier(value, source);
        modifiers.Add(modToAdd);
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(mod => mod.source == source);
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;
        foreach (StatModifier mod in modifiers)
        {
            finalValue += mod.value;
        }
        return finalValue;
    }

    public void SetBaseValue(float value)
    {
        baseValue = value;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
