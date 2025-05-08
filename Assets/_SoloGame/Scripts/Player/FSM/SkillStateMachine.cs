using UnityEngine;

public class SkillStateMachine : BaseStateMachine
{
    public IdleSkillState IdleState;
    public GooState UndergroundState;

    public SkillStateMachine(PlayerController playerController, Animator animator) : base(playerController, animator)
    {
        IdleState = new IdleSkillState(playerController, animator);
        UndergroundState = new GooState(playerController, animator);
    }

    public override void Start()
    {
        base.Start();
        SkillStateMachine = this;
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
