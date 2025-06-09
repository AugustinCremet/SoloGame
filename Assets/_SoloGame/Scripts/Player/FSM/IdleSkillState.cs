using UnityEngine;

public class IdleSkillState : BaseState
{
    public IdleSkillState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        Debug.Log("Enter Idle Skill State");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
