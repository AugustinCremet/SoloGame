using UnityEngine;

public class IdleMovementState : MovementBaseState
{
    public IdleMovementState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enter Idle Movement State");
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
