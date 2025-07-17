using UnityEngine;
using BehaviorTree;

public class TaskResetTimer : Node
{
    private string _key;
    public TaskResetTimer(string key)
    {
        _key = key;
    }

    public override NodeState Evaluate()
    {
        _blackboard.Set(_key, 0f);
        _state = NodeState.SUCCESS;
        return _state;
    }
}
