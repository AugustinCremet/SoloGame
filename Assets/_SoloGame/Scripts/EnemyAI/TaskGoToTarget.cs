using UnityEngine;
using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Vector3 _currentDestination = Vector3.zero;
    public TaskGoToTarget()
    {
    }

    public override NodeState Evaluate()
    {
        _context.NavAgent.isStopped = false;
        if(_context.NavAgent.remainingDistance > _context.NavAgent.stoppingDistance || _currentDestination == Vector3.zero)
        {
            _currentDestination = _context.PlayerTransform.position;
            _context.NavAgent.SetDestination(_context.PlayerTransform.position);
        }

        if (!_context.NavAgent.pathPending && _context.NavAgent.remainingDistance <= _context.NavAgent.stoppingDistance)
        {
            _currentDestination = Vector3.zero;
            _state = NodeState.SUCCESS;
            return _state;
        }

        _state = NodeState.RUNNING;
        return _state;
    }
}
