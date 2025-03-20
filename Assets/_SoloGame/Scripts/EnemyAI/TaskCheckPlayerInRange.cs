using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public class TaskCheckPlayerInRange : Node
{
    public TaskCheckPlayerInRange()
    {

    }
    public override NodeState Evaluate()
    {
        if(UtilityFunctions.IsPlayerInSight(_context.EnemyTransform.position, _context.PlayerTransform))
        {
            Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.green);
            _context.NavAgent.isStopped = true;
            _state = NodeState.SUCCESS;
        }
        else
        {
            Debug.DrawRay(_context.EnemyTransform.position, _context.PlayerTransform.position - _context.EnemyTransform.position, Color.red);
            _context.NavAgent.isStopped = false;
            _state = NodeState.FAILURE;
        }

        //_state = NodeState.RUNNING;
        return _state;
    }
}
