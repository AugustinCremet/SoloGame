using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MovementStateMachine : BaseStateMachine
{
    public IdleMovementState IdleState;
    public MovingState MovingState;
    public MovementStateMachine(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
        IdleState = new IdleMovementState(playerController, animator);
        MovingState = new MovingState(playerController, animator);
    }

    public override void Start()
    {
        base.Start();
        MovementStateMachine = this;
    }

    protected override BaseState GetInitialState()
    {
        Debug.Log("Set initial state");
        return IdleState;
    }

    public override void SwitchState(BaseState state)
    {
        base.SwitchState(state);
    }
}
