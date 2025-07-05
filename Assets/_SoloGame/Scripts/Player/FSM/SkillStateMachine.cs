using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillStateMachine : BaseStateMachine
{
    public SkillStateMachine()
    {
        _transitionMap.Add(typeof(IdleSkillState), new List<Type> { typeof(MovingState), typeof(GooState), typeof(ShootingState), typeof(DeadState), typeof(HitState) } );
        _transitionMap.Add(typeof(MovingState),    new List<Type> { typeof(IdleSkillState), typeof(GooState), typeof(ShootingState), typeof(DeadState), typeof(HitState) });
        _transitionMap.Add(typeof(GooState),       new List<Type> { typeof(IdleSkillState), typeof(MovingState), typeof(DeadState) } );
        _transitionMap.Add(typeof(ShootingState),  new List<Type> { typeof(IdleSkillState), typeof(MovingState), typeof(GooState), typeof(DeadState), typeof(HitState) } );
        _transitionMap.Add(typeof(DeadState),      new List<Type> { });
        _transitionMap.Add(typeof(HitState),       new List<Type> { typeof(IdleSkillState), typeof(MovingState), typeof(GooState), typeof(ShootingState), typeof(DeadState)});
    }

    public override bool TryChangeState(BaseState state)
    {
        return base.TryChangeState(state);      
    }
}
