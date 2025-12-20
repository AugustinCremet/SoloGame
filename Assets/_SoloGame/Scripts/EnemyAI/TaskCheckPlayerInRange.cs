using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskCheckPlayerInRange : Node
{
    Transform _transform;
    Transform _playerTransform;
    LayerMask _layerMask;
    public TaskCheckPlayerInRange(Transform transform, Transform playerTransform, LayerMask layerMask)
    {
        _transform = transform;
        _playerTransform = playerTransform;
        _layerMask = layerMask;
    }
    public override NodeState Evaluate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_transform.position, _playerTransform.position - _transform.position, Mathf.Infinity, _layerMask);

        if (hit.collider != null)
        {
            bool hasLineOfSight = hit.collider.CompareTag("Player");
            if(hasLineOfSight)
            {
                Debug.DrawRay(_transform.position, _playerTransform.position - _transform.position, Color.green);
                EnemyAI.Agent.isStopped = true;
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                Debug.DrawRay(_transform.position, _playerTransform.position - _transform.position, Color.red);
                EnemyAI.Agent.isStopped = false;
                state = NodeState.FAILURE;
                return state;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
