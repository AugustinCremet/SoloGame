using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckPlayerInRange : Node
{
    Transform transform;
    Transform playerTransform;
    LayerMask layerMask;
    public CheckPlayerInRange(Transform transform, Transform playerTransform, LayerMask layerMask)
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.layerMask = layerMask;
    }
    public override NodeState Evaluate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position - transform.position, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            bool hasLineOfSight = hit.collider.CompareTag("Player");
            if(hasLineOfSight)
            {
                Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.green);
                EnemyAI.agent.isStopped = true;
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.red);
                EnemyAI.agent.isStopped = false;
                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}
