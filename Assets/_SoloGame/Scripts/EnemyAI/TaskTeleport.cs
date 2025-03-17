using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class TaskTeleport : Node
{
    private bool _withinPlayerSight;
    private Bounds _navBounds;
    public TaskTeleport(bool withinPlayerSight)
    {
        _navBounds = GameObject.Find("BG").GetComponent<TilemapRenderer>().bounds;
    }

    public override NodeState Evaluate()
    {
        float randomX = Random.Range(_navBounds.min.x, _navBounds.max.x);
        float randomZ = Random.Range(_navBounds.min.y, _navBounds.max.y);
        Vector2 newLocation = new Vector2(randomX, randomZ);

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
                    _state = NodeState.SUCCESS;
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
}
