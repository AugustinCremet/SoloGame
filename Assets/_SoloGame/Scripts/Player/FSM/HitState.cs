using UnityEngine;
using UnityEngine.Rendering;

public class HitState : ActionBaseState
{
    private float _hitDuration = 0.5f;
    public override bool CanExit => _hitDuration <= 0f;

    public HitState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState()
    {
        _animator.SetBool("IsHit", true);
        _animator.Play("Hit");
    }

    public override void ExitState()
    {
        _hitDuration = 0.5f;
        _animator.SetBool("IsHit", false);
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        _hitDuration -= Time.deltaTime;

        if (_hitDuration <= 0f)
        {
            _player.StartInvincibility();
            _player.ReevaluateState();
        }
    }
}
