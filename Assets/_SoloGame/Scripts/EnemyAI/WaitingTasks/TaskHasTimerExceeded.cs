using UnityEngine;
using BehaviorTree;

public class TaskHasTimerExceeded : Node
{
    private float _threshold;
    public TaskHasTimerExceeded(float timerThreshold)
    {
        _threshold = timerThreshold;
    }

    public override NodeState Evaluate()
    {
        _state = _context.Enemy.IsShootingTimeDone(_threshold) ? NodeState.SUCCESS : NodeState.FAILURE;
        Debug.Log(_state);
        return _state;
    }
}
