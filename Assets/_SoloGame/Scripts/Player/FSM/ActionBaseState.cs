using UnityEngine;

public abstract class ActionBaseState : BaseState
{
    protected ActionBaseState(PlayerController playerController, Player player, Animator animator) : base(playerController, player, animator)
    {
    }
}
