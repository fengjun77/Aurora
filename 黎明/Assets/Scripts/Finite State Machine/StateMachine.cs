using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }

    public bool canEnterNewState;

    public void Init(EntityState state)
    {
        canEnterNewState = true;
        currentState = state;
        currentState.Enter();
    }

    //切换状态
    public void ChangeState(EntityState newState)
    {
        if(!canEnterNewState)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    //更新当前状态
    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => canEnterNewState = false;
}
