using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }

    public void Init(EntityState state)
    {
        currentState = state;
        currentState.Enter();
        
    }

    //切换状态
    public void ChangeState(EntityState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //更新当前状态
    public void UpdateActiveState()
    {
        currentState.Update();
    }
}
