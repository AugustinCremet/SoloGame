using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseStateMachine<TState> where TState : BaseState
{
    public TState CurrentState { get; private set; }

    protected BaseStateMachine()
    {
        
    }
    public virtual void SetInitialState(TState state)
    {
        CurrentState = state;
        CurrentState?.EnterState();
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdateState();
    }

    public void ResetStates(TState state)
    {
        SwitchState(state);
    }

    public virtual bool TryChangeState(TState state, PlayerStatus status)
    {
        if (CurrentState != null && !CurrentState.CanExit)
            return false;

        if(status.IsDead || status.IsStunned)
            return false;

        if(CurrentState == state)
            return false;

        SwitchState(state);
        return true;
    }

    private void SwitchState(TState state)
    {
        CurrentState?.ExitState();
        CurrentState = state;
        state.EnterState();
    }

    protected virtual TState GetInitialState()
    {
        Debug.Log("Null");
        return null;
    }
}
