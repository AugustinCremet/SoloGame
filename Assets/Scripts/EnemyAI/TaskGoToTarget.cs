using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : Node
{
    Transform playerTransform;

    public TaskGoToTarget(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    public override NodeState Evaluate()
    {
        EnemyAI.agent.SetDestination(playerTransform.position);
        state = NodeState.SUCCESS;
        return state;
    }
}
