using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillStateMachine : BaseStateMachine
{
    public SkillStateMachine(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
        //IdleState = new IdleSkillState(playerController, animator);
        //GooState = new GooState(playerController, animator);

        _transitionMap.Add(typeof(IdleSkillState), new List<Type> { typeof(GooState), typeof(ShootingState) } );
        _transitionMap.Add(typeof(GooState),       new List<Type> { typeof(IdleSkillState) } );
        _transitionMap.Add(typeof(ShootingState),  new List<Type> { typeof(IdleSkillState), typeof(GooState) } );
    }

    public override void TryChangeState(BaseState state)
    {
        base.TryChangeState(state);
    }
}
