using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine<TState> : BaseStateMachine<TState> where TState : BaseState
{
    public StateMachine()
    {
    }

    public override bool TryChangeState(TState state, PlayerStatus status)
    {
        return base.TryChangeState(state, status);      
    }
}
