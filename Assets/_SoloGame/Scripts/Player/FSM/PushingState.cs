using UnityEngine;

public class PushingState : MovementBaseState
{
    private PushBlock _pushBlock;
    private Vector3 _dir;
    public PushingState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState()
    {
        Vector3 checkPos = _player.transform.position + (Vector3)_playerController.MovementVector * 0.6f;
        Collider2D hit = Physics2D.OverlapCircle(checkPos, 0.2f, 1 << 19); // small radius
        if (hit != null && hit.TryGetComponent(out PushBlock pushBlock))
        {
            if(pushBlock.CanItBePushed(_playerController.MovementVector))
            {
                pushBlock.StartPushing(_playerController.MovementVector, 1f);
                _pushBlock = pushBlock;
                _dir = (Vector3)_playerController.MovementVector;
            }
            else
            {
                _player.MovementSM.TryChangeState(_player.MovingState, _player.Status);
            }
        }
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        if(_pushBlock != null)
        {
            _player.transform.position = _pushBlock.transform.position - _dir;

            if(!_pushBlock.IsMoving())
            {
                _player.MovementSM.TryChangeState(_player.IdleMovementState, _player.Status);
            }
        }
    }
}
