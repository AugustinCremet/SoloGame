using UnityEngine;

public class MovingState : BaseState
{
    public MovingState(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsMoving", true);
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsMoving", false);
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
