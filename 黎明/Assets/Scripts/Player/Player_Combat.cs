using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("反击参数")]
    //反击持续时间
    [SerializeField] private float counterDuration;

    public bool CounterAttack()
    {
        bool hasPerformedCounter = false;

        //获取目标身上的反击接口函数，执行函数造成敌人击退以及眩晕
        foreach(var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            //如果当前目标没有对应接口，则继续遍历下一个
            if(counterable == null)
                continue;
            //如果当前目标当前可被反击，则执行反击逻辑
            if(counterable.CanbeCounter)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }

        return hasPerformedCounter;
    }

    public float GetCounterDuration() => counterDuration;
}
