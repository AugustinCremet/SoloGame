using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskMoveBetween : Node
{
    private float _movementRangeFactor = 0.5f;
    private float _maxPerpendicularDistance = 2f;
    private Vector3 _currentDestination = Vector3.zero;

    public TaskMoveBetween()
    {
    }

    public override NodeState Evaluate()
    {
        Vector2 directionToPlayer = (_context.PlayerTransform.position - _context.EnemyTransform.position).normalized;
        float distanceToPlayer = Vector3.Distance(_context.EnemyTransform.position, _context.PlayerTransform.position);

        // Find a perpendicular direction
        Vector2 perpendicularDir = new Vector2(-directionToPlayer.y, directionToPlayer.x); // Always perpendicular on XZ plane

        // Generate a random position along the player direction (clamped within the rectangle)
        float moveFactor = Random.Range(0f, _movementRangeFactor); // 0 = enemy position, 1 = player position
        Vector2 alongPlayer = (Vector2)_context.EnemyTransform.position + directionToPlayer * (moveFactor * distanceToPlayer);

        // Generate a perpendicular movement within the allowed range
        float perpendicularOffset = Random.Range(-_maxPerpendicularDistance, _maxPerpendicularDistance);
        Vector2 finalPosition = alongPlayer + perpendicularDir * perpendicularOffset;

        // Ensure the position is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(finalPosition, out hit, 1.0f, NavMesh.AllAreas) && _currentDestination == Vector3.zero)
        {
            _currentDestination = finalPosition;
            _context.NavAgent.SetDestination(hit.position);
        }

        if(_context.NavAgent.remainingDistance <= _context.NavAgent.stoppingDistance && _currentDestination != Vector3.zero)
        {
            _currentDestination = Vector3.zero;
            _state = NodeState.SUCCESS;
            return _state;
        }

        _state = NodeState.FAILURE;
        return _state;
    }
}
