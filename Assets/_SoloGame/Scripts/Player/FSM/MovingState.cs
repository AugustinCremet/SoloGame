using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MovingState : BaseState
{
    private float _lastPositionX;
    private float _lastPositionY;
    private Vector2 _lastDirection;
    private float _pushTimer;

    private const float _PUSH_DELAY = 0.2f;
    public MovingState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.Play("Movement");
        _animator.SetBool("IsMoving", true);

        _lastPositionX = _player.transform.position.x;
        _lastPositionY = _player.transform.position.y;
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        //_player.ResetMovementVector();
        _animator.SetBool("IsMoving", false);
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        if(_lastPositionX != _player.transform.position.x ||
           _lastPositionY != _player.transform.position.y)
        {
            _lastPositionX = _player.transform.position.x;
            _lastPositionY = _player.transform.position.y;
            _pushTimer = 0;
        }
        else
        {
            _pushTimer += Time.fixedDeltaTime;

            // Prevent diagonal pushing
            if (_playerController.MovementVector.x != 0f && _playerController.MovementVector.y != 0f)
                return;

            if (_pushTimer > _PUSH_DELAY)
            {
                Vector2 move = _playerController.MovementVector;

                if (move.sqrMagnitude < 0.001f)
                    return;

                Vector2 dir = move.normalized;
                Vector2 origin = (Vector2)_player.transform.position + dir * 0.1f;
                float distance = 1.3f;

                int layerMask = LayerMask.GetMask("Pushable");

                RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);
                Debug.DrawRay(origin, dir * distance, Color.red);

                if (hit.collider != null && hit.collider.TryGetComponent(out PushBlock pushBlock))
                {
                    _player.StateMachine.TryChangeState(_player.PushingState);
                }
            }
        }
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _animator.SetFloat("Horizontal", _playerController.MovementVector.x);
        _animator.SetFloat("Vertical", _playerController.MovementVector.y);
    }
}
