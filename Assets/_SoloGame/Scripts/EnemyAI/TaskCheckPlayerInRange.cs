using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskCheckPlayerInRange : Node
{
    LayerMask _layerMask;
    private bool _doOnce = false;
    public TaskCheckPlayerInRange()
    {

    }
    public override NodeState Evaluate()
    {
        if(!_doOnce)
        {
            _layerMask = 1 << _context.PlayerTransform.gameObject.layer;
            _layerMask |= 1 << 9;
            _doOnce = true;
        }

        RaycastHit2D hit = Physics2D.Raycast(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Mathf.Infinity, _layerMask);

        if (hit.collider != null)
        {
            bool hasLineOfSight = hit.collider.CompareTag("Player");
            if(hasLineOfSight)
            {
                Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.green);
                _context.NavAgent.isStopped = true;
                _state = NodeState.SUCCESS;
                return _state;
            }
            else
            {
                Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.red);
                _context.NavAgent.isStopped = false;
                _state = NodeState.FAILURE;
                return _state;
            }
        }

        _state = NodeState.RUNNING;
        return _state;
    }
}
