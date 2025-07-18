using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskStopAttack : Node
{
    private float _currentWaitTime;
    private float _timeToWait = 0.1f;
    public TaskStopAttack()
    {
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
            _context.Enemy.StopAttack();
        }
        else
        {
            _state = NodeState.RUNNING;
        }

        return _state;
    }
}
