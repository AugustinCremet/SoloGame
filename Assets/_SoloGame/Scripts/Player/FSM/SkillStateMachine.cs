using UnityEngine;

public class SkillStateMachine : BaseStateMachine
{
    public IdleSkillState IdleState;
    public UndergroundState UndergroundState;

    private void Awake()
    {
        SkillStateMachine = this;
        IdleState = new IdleSkillState();
        UndergroundState = new UndergroundState();
    }

    protected override BaseState GetInitialState()
    {
        return IdleState;
    }

    public override void SwitchState(BaseState state)
    {
        base.SwitchState(state);
    }
}
