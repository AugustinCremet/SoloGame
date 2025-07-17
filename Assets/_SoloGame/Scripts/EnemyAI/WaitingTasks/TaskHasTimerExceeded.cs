using UnityEngine;
using BehaviorTree;

public class TaskHasTimerExceeded : Node
{
    private string _key;
    private float _threshold;
    public TaskHasTimerExceeded(string key, float timerThreshold)
    {
        _key = key; 
        _threshold = timerThreshold;
    }

    public override NodeState Evaluate()
    {
        _state = _blackboard.Get<float>(_key) >= _threshold ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;
    }
}
