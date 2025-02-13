using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.AI;
using UnityEngine.AI;

public class TaskCheckIfPlayerOnTheNav : Node
{
    public TaskCheckIfPlayerOnTheNav()
    {
    }
    public override NodeState Evaluate()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(_context.PlayerTransform.position, out hit, 1f, NavMesh.AllAreas))
        {
            _state = NodeState.SUCCESS;
            return _state;
        }
        else
        {
            _state = NodeState.FAILURE;
            return _state;
        }
    }
}
