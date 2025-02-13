using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskStopAttack : Node
{
    public TaskStopAttack()
    {
    }
    public override NodeState Evaluate()
    {
        _context.Enemy.StopAttack();
        _state = NodeState.SUCCESS;
        return _state;
    }
}
