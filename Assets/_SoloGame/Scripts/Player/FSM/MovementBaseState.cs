using UnityEngine;

public abstract class MovementBaseState : BaseState
{
    protected MovementBaseState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }
}
