using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TaskMoveBetween : Node
{
    private float _minMovementRangeFactor = 0.0f;
    private float _maxMovementRangeFactor = 0.5f;
    private float _maxPerpendicularDistance = 10f;
    private Vector3 _currentDestination = Vector3.zero;

    public TaskMoveBetween(float minMovementRangeFactor, float maxMovementRangeFactor, float maxPerpendicularDistance)
    {
        _minMovementRangeFactor = minMovementRangeFactor;
        _maxMovementRangeFactor = maxMovementRangeFactor;
        _maxPerpendicularDistance = maxPerpendicularDistance;
    }

    public override NodeState Evaluate()
    {
        Vector2 directionToPlayer = (_context.PlayerTransform.position - _context.EnemyTransform.position).normalized;
        float distanceToPlayer = Vector3.Distance(_context.EnemyTransform.position, _context.PlayerTransform.position);

        // Find a perpendicular direction
        Vector2 perpendicularDir = new Vector2(-directionToPlayer.y, directionToPlayer.x); // Always perpendicular on XZ plane

        // Generate a random position along the player direction (clamped within the rectangle)
        float moveFactor = Random.Range(_minMovementRangeFactor, _maxMovementRangeFactor); // 0 = enemy position, 1 = player position
        Vector2 alongPlayer = (Vector2)_context.EnemyTransform.position + directionToPlayer * moveFactor * distanceToPlayer;

        // Generate a perpendicular movement within the allowed range
        float perpendicularOffset = Random.Range(-_maxPerpendicularDistance, _maxPerpendicularDistance);
        Vector2 finalPosition = alongPlayer + perpendicularDir * perpendicularOffset;

        if(_context.NavAgent.remainingDistance <= _context.NavAgent.stoppingDistance && _currentDestination != Vector3.zero)
        {
            _currentDestination = Vector3.zero;
            _state = NodeState.SUCCESS;
            return _state;
        }

        // Ensure the position is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(finalPosition, out hit, 1.0f, NavMesh.AllAreas) && _currentDestination == Vector3.zero)
        {
            float movingDistance = Vector2.Distance(_context.EnemyTransform.position, finalPosition);
            if(movingDistance > _context.NavAgent.stoppingDistance)
            {
                _currentDestination = finalPosition;
                _context.NavAgent.SetDestination(hit.position);
            }
            DrawRectangle(perpendicularDir, directionToPlayer, distanceToPlayer);
            Debug.DrawLine(_context.NavAgent.transform.position, finalPosition, Color.red, 1f);
        }


        _state = NodeState.RUNNING;
        return _state;
    }

    // For debugging
    public void DrawRectangle(Vector2 perpendicularDir, Vector2 playerDir, float playerDistance)
    {
        Vector3 nearPerpendicularLeftSide;
        Vector3 nearPerpendicularRightSide;
        Vector3 farPerpendicularLeftSide;
        Vector3 farPerpendicularRightSide;
        Vector2 recEndPoint;

        recEndPoint = (Vector2)_context.NavAgent.transform.position + playerDir * playerDistance * _maxMovementRangeFactor;

        nearPerpendicularLeftSide = (Vector2)_context.NavAgent.transform.position + perpendicularDir * _maxPerpendicularDistance;
        nearPerpendicularRightSide = (Vector2)_context.NavAgent.transform.position + perpendicularDir * -_maxPerpendicularDistance;
        farPerpendicularLeftSide = recEndPoint + perpendicularDir * _maxPerpendicularDistance;
        farPerpendicularRightSide = recEndPoint + perpendicularDir * -_maxPerpendicularDistance;

        Debug.DrawLine(nearPerpendicularRightSide, nearPerpendicularLeftSide, Color.green, 1f);
        Debug.DrawLine(farPerpendicularRightSide, farPerpendicularLeftSide, Color.green, 1f);
        Debug.DrawLine(nearPerpendicularLeftSide, farPerpendicularLeftSide, Color.green, 1f);
        Debug.DrawLine(nearPerpendicularRightSide, farPerpendicularRightSide, Color.green, 1f);
    }
}
