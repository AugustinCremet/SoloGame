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
            _hasAttacked = true;
            _state = NodeState.SUCCESS;
        }
        else
        {
            _state = NodeState.RUNNING;
        }
        return _state;
    }

    public override void Reset()
    {
        base.Reset();
        _hasAttacked = false;
    }
}
