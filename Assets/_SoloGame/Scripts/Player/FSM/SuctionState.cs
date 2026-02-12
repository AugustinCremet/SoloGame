using System.Collections;
using UnityEngine;

public class SuctionState : ActionBaseState
{
    public SuctionState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
        BlockMovement = true;
    }

    public override void EnterState()
    {
        _animator.SetBool("IsSuctionOver", false);
        _animator.Play("Suction_Into");
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {
    }

    public override void UpdateState()
    {
        _player.HandleSuction();
    }
}
