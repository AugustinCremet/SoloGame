using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    private bool _hasAttacked = false;
    public TaskAttack()
    {
    }
    public override NodeState Evaluate()
    {
        if(!_hasAttacked)
        {
            _context.Enemy.StartAttack();
        }

        if (_context.Enemy.IsEmitterPlaying())
        {
            Debug.Log("Attack is happening");
            _hasAttacked = true;
            _state = NodeState.SUCCESS; // or RUNNING if you want to wait here
        }
        else
        {
            Debug.Log("Waiting for emitter to start...");
            _state = NodeState.RUNNING;
        }
        return _state;
    }

    public override void Reset()
    {
        base.Reset();
        Debug.Log("reset");
        _hasAttacked = false;
    }
}
