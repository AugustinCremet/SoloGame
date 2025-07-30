using BehaviorTree;
using UnityEngine;

public class TaskIsAttackOver : Node
{
    public override NodeState Evaluate()
    {
        if(_context.Enemy.IsEmitterPlaying())
        {
            Debug.Log("Attack is not over");
            _state = NodeState.RUNNING;
        }
        else
        {
            _context.Enemy.StopAttack();
            _state = NodeState.SUCCESS;
        }

        return _state;  
    }
}
