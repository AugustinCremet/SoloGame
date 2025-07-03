using UnityEngine;

public class ShootingState : BaseState
{
    public override bool BlockMovement => true;
    private GameObject _aimSight;
    private Vector2 _aimSightDirection;
    private bool _hasShot = false;
    public ShootingState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _aimSight = GameObject.FindWithTag("AimSight");

        _aimSightDirection = AimDirection();
        _animator.SetFloat("HorizontalAim", _aimSightDirection.x);
        _animator.SetFloat("VerticalAim", _aimSightDirection.y);

        _animator.SetBool("IsShooting", true);
    }

    private Vector2 AimDirection()
    {
        Vector2 aimDirection = (_aimSight.transform.position - _player.transform.position).normalized;
        float horizontal = Mathf.Abs(aimDirection.x) > 0.1f ? Mathf.Sign(aimDirection.x) : 0f;
        float vertical = Mathf.Abs(aimDirection.y) > 0.1f ? Mathf.Sign(aimDirection.y) : 0f;

        return new Vector2(horizontal, vertical);
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
        Vector2 newAimDirection = AimDirection();

        if (newAimDirection != _aimSightDirection)
        {
            _aimSightDirection = newAimDirection;
            _animator.SetFloat("HorizontalAim", _aimSightDirection.x);
            _animator.SetFloat("VerticalAim", _aimSightDirection.y);
        }
    }
}
