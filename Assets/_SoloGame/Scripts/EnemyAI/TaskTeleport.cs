using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class TaskTeleport : Node
{
    private bool _withinPlayerSight;
    private float _minDistanceFromPlayer;
    private Bounds _navBounds;
    private bool _doOnce;
    private int _layerMask;

    public TaskTeleport(bool withinPlayerSight, float minDistanceFromPlayer)
    {
        _withinPlayerSight = withinPlayerSight;
        _minDistanceFromPlayer = minDistanceFromPlayer;
        _navBounds = GameObject.Find("BG").GetComponent<TilemapRenderer>().bounds;
    }

    public override NodeState Evaluate()
    {
        float randomX = Random.Range(_navBounds.min.x, _navBounds.max.x);
        float randomZ = Random.Range(_navBounds.min.y, _navBounds.max.y);
        Vector2 newLocation = new Vector2(randomX, randomZ);

        if(Vector2.Distance(newLocation, _context.PlayerTransform.position) < _minDistanceFromPlayer)
        {
            _state = NodeState.RUNNING;
            return _state;
        }

        int walkableMask = 1 << NavMesh.GetAreaFromName("Walkable");

        NavMeshHit hit;
        if(NavMesh.SamplePosition(newLocation, out hit, 0.1f, walkableMask))
        {
            if (NavMesh.FindClosestEdge(hit.position, out NavMeshHit edgeHit, walkableMask))
            {
                if (edgeHit.distance >= 0.5f)
                {
                    Debug.DrawLine(_context.NavAgent.transform.position, newLocation, Color.green, 1f);
                    _context.NavAgent.Warp(newLocation);
                    _state = _withinPlayerSight ? IsPlayerInSight(hit) : _state = NodeState.SUCCESS;
                }
                else
                {
                    Debug.DrawLine(_context.NavAgent.transform.position, newLocation, Color.yellow, 1f);
                    _state = NodeState.RUNNING;
                }
            }
        }
        else
        {
            Debug.DrawLine(_context.NavAgent.transform.position, newLocation, Color.red, 1f);
            _state = NodeState.RUNNING;
        }

        return _state;
    }

    NodeState IsPlayerInSight(NavMeshHit hitPosition)
    {
        if (!_doOnce)
        {
            _layerMask = 1 << _context.PlayerTransform.gameObject.layer;
            _layerMask |= 1 << 9;
            _doOnce = true;
        }

        RaycastHit2D hit = Physics2D.Raycast(hitPosition.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Mathf.Infinity, _layerMask);

        if (hit.collider != null)
        {
            bool hasLineOfSight = hit.collider.CompareTag("Player");
            if (hasLineOfSight)
            {
                Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.blue, 1f);
                _context.NavAgent.isStopped = true;
                _state = NodeState.SUCCESS;
            }
            else
            {
                Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.magenta, 1f);
                _context.NavAgent.isStopped = false;
                _state = NodeState.RUNNING;
            }
        }

        return _state;
    }
}
