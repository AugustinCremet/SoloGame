using UnityEngine;

public class ShootingState : BaseState
{
    private bool _isFront;
    public ShootingState(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        if (GameObject.FindWithTag("AimSight").transform.position.y > _playerController.transform.position.y)
        {
            _animator.Play("ShootBack");
            _isFront = false;
        }
        else
        {
            _animator.Play("ShootFront");
            _isFront = true;
        }
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        if(_isFront)
        {
            _animator.Play("IdleFront");
        }
        else
        {
            _animator.Play("IdleBack");
        }
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {

    }
}
