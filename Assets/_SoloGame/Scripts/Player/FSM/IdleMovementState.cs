using UnityEngine;

public class IdleMovementState : BaseState
{
    public IdleMovementState(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        Debug.Log("Enter Idle Movement State");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
