using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillStateMachine : BaseStateMachine
{
    public SkillStateMachine()
    {
        _transitionMap.Add(typeof(IdleSkillState), new List<Type> { typeof(MovingState), typeof(GooState), typeof(ShootingState), typeof(DeadState) } );
        _transitionMap.Add(typeof(MovingState),    new List<Type> { typeof(IdleSkillState), typeof(GooState), typeof(ShootingState), typeof(DeadState) });
        _transitionMap.Add(typeof(GooState),       new List<Type> { typeof(IdleSkillState), typeof(DeadState) } );
        _transitionMap.Add(typeof(ShootingState),  new List<Type> { typeof(IdleSkillState), typeof(MovingState), typeof(GooState), typeof(DeadState) } );
        _transitionMap.Add(typeof(DeadState),      new List<Type> { });
    }

    public override void TryChangeState(BaseState state)
    {
        base.TryChangeState(state);
    }
}
