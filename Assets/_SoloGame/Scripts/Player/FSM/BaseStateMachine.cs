using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine
{
    protected BaseState _currentState;
    protected Dictionary<Type, List<Type>> _transitionMap = new Dictionary<Type, List<Type>>();

    protected BaseStateMachine(PlayerController playerController, Animator animator)
    {

    }
    public virtual void SetInitialState(BaseState state)
    {
        _currentState = state;
        _currentState?.EnterState(this);
    }

    public void Update()
    {
        _currentState?.UpdateState(this);
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdateState(this);
    }

    public virtual void TryChangeState(BaseState state)
    {
        var currentType = _currentState.GetType();
        var nextType = state.GetType();

        if (_transitionMap.TryGetValue(currentType, out var allowedStates) &&
            allowedStates.Contains(nextType))
        {
            SwitchState(state);
        }
    }

    private void SwitchState(BaseState state)
    {
        _currentState?.ExitState(this);
        _currentState = state;
        state.EnterState(this);
    }

    protected virtual BaseState GetInitialState()
    {
        Debug.Log("Null");
        return null;
    }
}
