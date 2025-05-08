using UnityEngine;

public abstract class BaseState
{
    protected PlayerController _playerController;
    protected Animator _animator;

    protected BaseState(PlayerController playerController, Animator animator)
    {
        _playerController = playerController;
        _animator = animator;
    }
    public abstract void EnterState(BaseStateMachine stateMachine);
    public abstract void UpdateState(BaseStateMachine stateMachine);

    public abstract void FixedUpdateState(BaseStateMachine stateMachine);
    public abstract void ExitState(BaseStateMachine stateMachine);
}
