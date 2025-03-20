using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    private bool _hasStartedAttack = false;
    public TaskAttack()
    {
    }
    public override NodeState Evaluate()
    {
        if (!_hasStartedAttack)
        {
            _context.Enemy.StartAttack();
            _hasStartedAttack = true;
            _state = NodeState.RUNNING;
        }
        else if(_context.Enemy.IsAttacking)
        {
            _state = NodeState.RUNNING;
        }
        else
        {
            _hasStartedAttack = false;
            _state = NodeState.SUCCESS;
        }

        return _state;
    }
}
