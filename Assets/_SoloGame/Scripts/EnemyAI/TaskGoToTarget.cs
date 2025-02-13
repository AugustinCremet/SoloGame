using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TaskGoToTarget : Node
{
    public TaskGoToTarget()
    {
    }

    public override NodeState Evaluate()
    {
        _context.NavAgent.SetDestination(_context.PlayerTransform.position);

        _state = NodeState.SUCCESS;
        return _state;
    }
}
