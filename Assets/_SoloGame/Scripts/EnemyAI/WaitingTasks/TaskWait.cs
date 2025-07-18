using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskWait : Node
{
    private float _timeToWait;
    private float _currentWaitTime;
    public TaskWait(float timeToWait)
    {
        _timeToWait = timeToWait;
    }
    public override NodeState Evaluate()
    {
        if (_currentWaitTime == 0f)
        {
            _currentWaitTime = Time.time; // Initialize the start time
        }

        if (Time.time - _currentWaitTime >= _timeToWait)
        {
            _currentWaitTime = 0f; // Reset for next use
            _state = NodeState.SUCCESS;
        }
        else
        {
            _state = NodeState.RUNNING;
        }

        return _state;
    }
}
