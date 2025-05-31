using UnityEngine;

public class IdleSkillState : BaseState
{
    public IdleSkillState(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        if(_playerController.IsGoo)
        {
            stateMachine.SwitchState(stateMachine.SkillStateMachine.GooState);
        }
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
