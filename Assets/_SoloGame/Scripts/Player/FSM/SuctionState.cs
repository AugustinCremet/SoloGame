using System.Collections;
using UnityEngine;

public class SuctionState : BaseState
{
    public SuctionState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState(BaseStateMachine stateMachine)
    {
        _animator.SetBool("IsSuctionOver", false);
        _animator.Play("Suction_Into");
    }

    public override void ExitState(BaseStateMachine stateMachine)
    {

    }

    public override void FixedUpdateState(BaseStateMachine stateMachine)
    {
    }

    public override void UpdateState(BaseStateMachine stateMachine)
    {
        _player.HandleSuction();
    }
}
