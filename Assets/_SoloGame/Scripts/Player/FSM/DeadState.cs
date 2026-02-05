using System;
using System.Collections;
using UnityEngine;

public class DeadState : ActionBaseState
{
    public DeadState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        _animator.Play("Death");
        _player.HandleDeath();
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new NotImplementedException();
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
