using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : Node
{
    Transform _playerTransform;

    public TaskGoToTarget(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public override NodeState Evaluate()
    {
        EnemyAI.Agent.SetDestination(_playerTransform.position);

        state = NodeState.SUCCESS;
        return state;
    }
}
