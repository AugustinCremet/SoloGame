using UnityEngine;
using BehaviorTree;

public class TaskIncrementTimer : Node
{
    private string _key;
    public TaskIncrementTimer(string bbKey)
    {
        _key = bbKey;
    }

    public override NodeState Evaluate()
    {
        float time = _blackboard.Get<float>(_key);
        time += Time.deltaTime;
        _blackboard.Set(_key, time);
        Debug.Log("Timer: " +  time);

        _state = NodeState.SUCCESS;
        return _state;
    }
}
