using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Entity_Stats statsToModify;

    [SerializeField] private float speed;
    [SerializeField] private float range;
    private Vector3 startPosition;

    [Header("属性增益设置")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private float buffDuration;
    [SerializeField] private string buffName;
    [SerializeField] private bool canBeUsed;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * speed) * range;
        transform.position = startPosition + new Vector3(0, yOffset);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!canBeUsed)
            return;
        
        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }

    private IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        sr.color = Color.clear;

        ApplyBuff(true);

        yield return new WaitForSeconds(duration);

        ApplyBuff(false);

        Destroy(gameObject);
    }

    /// <summary>
    /// 应用或移除Buff
    /// </summary>
    /// <param name="apply"></param>
    private void ApplyBuff(bool apply)
    {
        foreach(var buff in buffs)
        {
            if(apply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
