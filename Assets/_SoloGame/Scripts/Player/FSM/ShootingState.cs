using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShootingState : BaseState
{
    public override bool BlockMovement => true;
    public ShootingState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        Debug.Log("Enter Shooting State");
        var aimSight = GameObject.FindWithTag("AimSight");
        if (aimSight.transform.position.x > _playerController.transform.position.x)
        {
            _animator.SetFloat("HorizontalAim", 1f);
        }
        else if(aimSight.transform.position.x < _playerController.transform.position.x)
        {
            _animator.SetFloat("HorizontalAim", -1f);
        }

        if(aimSight.transform.position.y > _playerController.transform.position.y)
        {
            _animator.SetFloat("VerticalAim", 1f);
        }
        else if(aimSight.transform.position.y < _playerController.transform.position.y)
        {
            _animator.SetFloat("VerticalAim", -1f);
        }

        _animator.SetBool("IsShooting", true);
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsShooting", false);
        _animator.SetFloat("HorizontalAim", 0f);
        _animator.SetFloat("VerticalAim", 0f);
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
