using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    public TaskAttack()
    {
    }
    public override NodeState Evaluate()
    {
        _context.Enemy.StartAttack();
        _state = NodeState.SUCCESS;
        return _state;
    }
}
