using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    protected BaseState _currentState;

    [HideInInspector]
    public SkillStateMachine SkillStateMachine;
    [HideInInspector]
    public PlayerController PlayerController {  get; private set; }

    private void Awake()
    {
        Debug.Log("Awake");
    }
    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        _currentState = GetInitialState();
        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public virtual void SwitchState(BaseState state)
    {
        _currentState.ExitState(this);
        _currentState = state;
        state.EnterState(this);
    }

    protected virtual BaseState GetInitialState()
    {
        Debug.Log("Null");
        return null;
    }
}
