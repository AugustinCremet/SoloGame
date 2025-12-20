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
        _player.ResetMovementVector();
        _animator.SetBool("IsMoving", false);
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        _player.HandleMovement();

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

            if(_pushTimer > _PUSH_DELAY)
            {
                int layerMask = LayerMask.GetMask("Interactable");
                RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, (Vector3)_playerController.MovementVector.normalized, 1f, layerMask);
                Debug.DrawRay(_player.transform.position, _playerController.MovementVector.normalized, Color.red, 0.5f);
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
