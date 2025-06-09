using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementStateMachine : BaseStateMachine
{
    public MovementStateMachine()
    {

        _transitionMap.Add(typeof(IdleMovementState), new List<Type> { typeof(MovingState) });
        _transitionMap.Add(typeof(MovingState), new List<Type> { typeof(IdleMovementState) });
    }

    public override void TryChangeState(BaseState state)
    {
        base.TryChangeState(state);
    }
}
