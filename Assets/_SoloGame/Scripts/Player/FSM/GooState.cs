using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GooState : BaseState
{
    [SerializeField] float _timeBetweenSpawns;
    [SerializeField] float _startTimeBetweenSpawns;

    public GooState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.Play("ChangeToGoo");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _animator.SetFloat("Horizontal", _playerController.MovementVector.x);
        _animator.SetFloat("Vertical", _playerController.MovementVector.y);
        //_playerController.HandleGoo();
        _playerController.HandleMovement();
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
