using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.AI;
using UnityEngine.AI;

public class TaskCheckIfPlayerOnTheNav : Node
{
    Transform _target;

    public TaskCheckIfPlayerOnTheNav(Transform target)
    {
        _target = target;
    }
    public override NodeState Evaluate()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(_target.position, out hit, 1f, NavMesh.AllAreas))
        {
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
