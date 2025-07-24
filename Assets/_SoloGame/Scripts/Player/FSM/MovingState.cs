using UnityEngine;

public class MovingState : BaseState
{
    public MovingState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.Play("Movement");
        _animator.SetBool("IsMoving", true);
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        _player.StopMovement();
        _animator.SetBool("IsMoving", false);
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        _player.HandleMovement();
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _animator.SetFloat("Horizontal", _playerController.MovementVector.x);
        _animator.SetFloat("Vertical", _playerController.MovementVector.y);
    }
}
