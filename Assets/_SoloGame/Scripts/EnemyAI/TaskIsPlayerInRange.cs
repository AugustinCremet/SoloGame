using BehaviorTree;
using TMPro.EditorUtilities;
using UnityEngine;

public class TaskIsPlayerInRange : Node
{
    public override NodeState Evaluate()
    {
        float distanceToTarget = Vector2.Distance(_context.EnemyTransform.position, _context.PlayerTransform.position);
        if (distanceToTarget > _context.NavAgent.stoppingDistance)
        {
            _state = NodeState.FAILURE;
            return _state;
        }

        if (_context.NavAgent.remainingDistance <= _context.NavAgent.stoppingDistance && _context.NavAgent.hasPath)
        {
            _state = NodeState.SUCCESS;
        }
        else
        {
            _state = NodeState.FAILURE;
        }

        return _state;
    }
}
