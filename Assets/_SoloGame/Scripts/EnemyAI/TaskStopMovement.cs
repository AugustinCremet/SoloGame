using UnityEngine;
using BehaviorTree;

public class TaskStopMovement : Node
{
    public override NodeState Evaluate()
    {
        _context.NavAgent.isStopped = true;
        _state = NodeState.SUCCESS;
        return _state;
    }
}
