using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class BaseState
{
    protected PlayerController _playerController;
    protected Player _player;
    protected Animator _animator;

    public virtual bool BlockMovement => false;
    public virtual bool CanExit => true;

    protected BaseState(PlayerController playerController, Player player, Animator animator)
    {
        _playerController = playerController;
        _player = player;
        _animator = animator;
    }

    protected BaseState(PlayerController playerController, Animator animator)
    {
    }

    public abstract void EnterState(BaseStateMachine stateMachine);
    public abstract void UpdateState(BaseStateMachine stateMachine);

    public abstract void FixedUpdateState(BaseStateMachine stateMachine);
    public abstract void ExitState(BaseStateMachine stateMachine);
}
