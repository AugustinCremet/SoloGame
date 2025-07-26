using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseStateMachine
{
    public BaseState CurrentState { get; private set; }
    protected Dictionary<Type, List<Type>> _transitionMap = new Dictionary<Type, List<Type>>();

    protected BaseStateMachine()
    {
        
    }
    public virtual void SetInitialState(BaseState state)
    {
        CurrentState = state;
        CurrentState?.EnterState(this);
    }

    public void Update()
    {
        CurrentState?.UpdateState(this);
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdateState(this);
    }

    public void ResetStates(IdleSkillState state)
    {
        SwitchState(state);
    }

    public virtual bool TryChangeState(BaseState state)
    {
        if (CurrentState != null && !CurrentState.CanExit)
            return false;

        var currentType = CurrentState.GetType();
        var nextType = state.GetType();

        if (_transitionMap.TryGetValue(currentType, out var allowedStates) &&
            allowedStates.Contains(nextType))
        {
            SwitchState(state);
            return true;
        }
        return false;
    }

    private void SwitchState(BaseState state)
    {
        CurrentState?.ExitState(this);
        CurrentState = state;
        state.EnterState(this);
    }

    protected virtual BaseState GetInitialState()
    {
        Debug.Log("Null");
        return null;
    }
}
