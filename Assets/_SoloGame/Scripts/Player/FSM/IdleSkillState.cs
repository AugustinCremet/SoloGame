using UnityEngine;

public class IdleSkillState : BaseState
{
    public override void EnterState(BaseStateMachine stateMachine)
    {
        
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        if(stateMachine.PlayerController.IsDashing)
            stateMachine.SwitchState(stateMachine.SkillStateMachine.UndergroundState);
    }
}
