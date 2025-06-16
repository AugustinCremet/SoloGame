using System;
using System.Collections;
using UnityEngine;

public class DeadState : BaseState
{
    public DeadState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _animator.Play("Death");
        _player.HandleDeath();
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {
        
    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
        
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        
    }
}
