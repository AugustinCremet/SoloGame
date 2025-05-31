using UnityEngine;

public class BaseStateMachine
{
    protected BaseState _currentState;

    [HideInInspector]
    public SkillStateMachine SkillStateMachine;
    [HideInInspector]
    public MovementStateMachine MovementStateMachine;

    protected BaseStateMachine(PlayerController playerController, Animator animator)
    {

    }
    public virtual void Start()
    {
        _currentState = GetInitialState();
        _currentState?.EnterState(this);
    }

    public void Update()
    {
        _currentState?.UpdateState(this);
    }

    public void FixedUpdate()
    {
        _currentState?.FixedUpdateState(this);
    }

    public virtual void SwitchState(BaseState state)
    {
        _currentState?.ExitState(this);
        _currentState = state;
        state.EnterState(this);
    }

    protected virtual BaseState GetInitialState()
    {
        Debug.Log("Null");
        return null;
    }
}
