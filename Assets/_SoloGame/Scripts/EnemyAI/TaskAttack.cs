using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : Node
{
    bool isCurrentlyPlaying = false;
    public TaskAttack()
    {
    }
    public override NodeState Evaluate()
    {
        if (!isCurrentlyPlaying)
        {
            isCurrentlyPlaying = _context.Enemy.Attack();
            _state = NodeState.SUCCESS;
        }
        else
        {
            _state = NodeState.SUCCESS;
        }
        return _state;
    }
}
